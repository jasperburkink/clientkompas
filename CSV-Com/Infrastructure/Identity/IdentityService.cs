﻿using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class IdentityService(
        UserManager<AuthenticationUser> userManager,
        SignInManager<AuthenticationUser> signInManager,
        RoleManager<AuthenticationRole> roleManager,
        IUserClaimsPrincipalFactory<AuthenticationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        IAuthenticationDbContext authenticationDbContext,
        IHasher hasher,
        ITokenService refreshTokenService,
        IEmailService emailService) : IIdentityService
    {
        private const string WEBAPP_URL = "http://localhost:3000"; // TODO: Move this url to the appsettings or get the url from constants?
        private const string TOKEN_PROVIDER = "Email";
        private const bool TWOFACTORAUTHENTICATION_DEFAULT_ENABLED = true;

        public async Task<string?> GetUserNameAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            return user?.UserName;
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, int cvsUserId)
        {
            var user = new AuthenticationUser
            {
                UserName = userName,
                Email = userName,
                TwoFactorEnabled = TWOFACTORAUTHENTICATION_DEFAULT_ENABLED, // NOTE: Two-factor authentication is turned on by default
                CVSUserId = cvsUserId,
                HasTemporaryPassword = true,
                TemporaryPasswordExpiryDate = DateTime.UtcNow.Add(TemporaryPasswordTokenConstants.TOKEN_TIMEOUT),
                TemporaryPasswordTokenCount = 1
            };

            var result = await userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);

            return user != null && await userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var principal = await userClaimsPrincipalFactory.CreateAsync(user);

            var result = await authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            return user != null ? await DeleteUserAsync(user) : Result.Success();
        }

        public async Task<Result> DeleteUserAsync(AuthenticationUser user)
        {
            var result = await userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<LoggedInResult> LoginAsync(string userName, string password)
        {
            // TODO: Look at cookie and lockedout parameter
            var result = await signInManager.PasswordSignInAsync(userName, password, true, false);
            if (!result.Succeeded)
            {
                return new LoggedInResult(result.Succeeded);
            }

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new LoggedInResult(result.Succeeded);
            }

            var roles = await userManager.GetRolesAsync(user);
            return new LoggedInResult(result.Succeeded, user, roles);
        }

        public async Task LogoutAsync()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IList<string>> GetRolesAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return [];
            }

            return await userManager.GetRolesAsync(user);
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            return await userManager.FindByIdAsync(userId) != null;
        }

        public async Task<IAuthenticationUser> GetUserAsync(string userId) => await userManager.FindByIdAsync(userId);

        public async Task<Result> SendResetPasswordEmailAsync(string emailAddress)
        {
            var user = await userManager.FindByEmailAsync(emailAddress);

            // When user is unknown, still show a success result. No polling if emailaddresses are in the system.
            if (user == null)
            {
                return Result.Success();
            }

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var encodedToken = Uri.EscapeDataString(token);

            // TODO: Email logic should not be in this class
            var link = new Uri($"{WEBAPP_URL}/reset-password/{emailAddress}/{encodedToken}");

            await emailService.SendEmailAsync(emailAddress, "Wachtwoord opnieuw instellen", // TODO: Use the new emailservice and take the text from resources.
                $""""
                Via deze link kunt U uw wachtwoord opnieuw instellen.
                {link}
                """");

            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(string emailAddress, string token, string newPassword)
        {
            var user = await userManager.FindByEmailAsync(emailAddress);

            if (user == null)
            {
                return Result.Failure(["User is not found with the given emailaddress."]);
            }

            var result = await userManager.ResetPasswordAsync(user, token, newPassword);

            return result.ToApplicationResult();
        }

        public async Task<string> Get2FATokenAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId)
                ?? throw new Application.Common.Exceptions.NotFoundException("AuthenticationUser not found.", userId);

            return await userManager.GenerateTwoFactorTokenAsync(user, TOKEN_PROVIDER);
        }

        public async Task<LoggedInResult> Login2FAAsync(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId)
                ?? throw new Application.Common.Exceptions.NotFoundException("AuthenticationUser not found.", userId);

            var tokenValid = await userManager.VerifyTwoFactorTokenAsync(user, TOKEN_PROVIDER, token);

            var roles = await userManager.GetRolesAsync(user);

            return new LoggedInResult(tokenValid, user, roles);
        }

        public async Task<int?> GetCurrentLoggedInUserId()
        {
            var user = await signInManager.UserManager.GetUserAsync(signInManager.Context.User);
            return user?.CVSUserId;
        }

        public async Task<Result> AddUserToRoleAsync(string userId, string role)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.Failure($"AuthenticationUser with user id '{userId}' not found.");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                return Result.Failure($"Role '{role}' not found.");
            }

            await userManager.AddToRoleAsync(user, role);

            return Result.Success();
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            return user == null
                ? throw new Application.Common.Exceptions.NotFoundException("AuthenticationUser not found.", userId)
                : await userManager.GetRolesAsync(user);
        }

        public async Task RemoveUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId)
                ?? throw new Application.Common.Exceptions.NotFoundException("AuthenticationUser not found.", userId);
            await userManager.DeleteAsync(user);
        }

        public async Task<IList<string>> GetAvailableUserRolesAsync()
        {
            return await Task.FromResult(roleManager.Roles
                .Select(role => role.Name!)
                .ToList());
        }

        public async Task UpdateUserAsync(IAuthenticationUser user)
        {
            if (user is not AuthenticationUser authUser)
            {
                throw new InvalidOperationException("User is not of type AuthenticationUser");
            }

            await userManager.UpdateAsync(authUser);
        }

        public async Task<IList<IAuthenticationUser>> GetUsersInRolesAsync(string role, params string[] roles)
        {
            var allRoles = new HashSet<string>(roles) { role };

            var users = await (from user in authenticationDbContext.Users
                               join userRole in authenticationDbContext.UserRoles on user.Id equals userRole.UserId
                               join roleEntity in authenticationDbContext.Roles on userRole.RoleId equals roleEntity.Id
                               select new { user, roleEntity.Name }).ToListAsync();

            return [.. users
                .Where(roleUser => allRoles.Contains(roleUser.Name))
                .Select(roleUser => (IAuthenticationUser)roleUser.user)
                .Distinct()];
        }
    }
}
