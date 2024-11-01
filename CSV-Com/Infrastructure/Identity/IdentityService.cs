using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Domain.Authentication.Domain;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<AuthenticationUser> _userManager;
        private readonly SignInManager<AuthenticationUser> _signInManager;
        private readonly IUserClaimsPrincipalFactory<AuthenticationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly IHasher _hasher;
        private readonly IRefreshTokenService _refreshTokenService;
        private const string WEBAPP_URL = "http://localhost:3000"; // TODO: Move this url to the appsettings or get the url from constants?

        public IdentityService(
            UserManager<AuthenticationUser> userManager,
            SignInManager<AuthenticationUser> signInManager,
            IUserClaimsPrincipalFactory<AuthenticationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService,
            IHasher hasher,
            IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            _hasher = hasher;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<string?> GetUserNameAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.UserName;
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
        {
            var user = new AuthenticationUser
            {
                UserName = userName,
                Email = userName
            };

            var result = await _userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user != null && await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user != null ? await DeleteUserAsync(user) : Result.Success();
        }

        public async Task<Result> DeleteUserAsync(AuthenticationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }

        public async Task<LoggedInResult> LoginAsync(string userName, string password)
        {
            // TODO: Look at cookie and lockedout parameter
            var result = await _signInManager.PasswordSignInAsync(userName, password, true, false);
            if (!result.Succeeded)
            {
                return new LoggedInResult(result.Succeeded);
            }

            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return new LoggedInResult(result.Succeeded);
            }

            var roles = await _userManager.GetRolesAsync(user);
            return new LoggedInResult(result.Succeeded, user, roles);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IList<string>> GetRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new List<string>();
            }

            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> UserExistsAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId) != null;
        }

        public async Task<AuthenticationUser> GetUserAsync(string userId) => await _userManager.FindByIdAsync(userId);

        public async Task<Result> SendResetPasswordEmailAsync(string emailAddress)
        {
            var user = await _userManager.FindByEmailAsync(emailAddress);

            if (user == null)
            {
                return Result.Success();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = new Uri($"{WEBAPP_URL}/ResetPassword?Token={token}");

            var emailService = new EmailService(); // TODO: Use the new emailservice and take the text from resources.
            emailService.SendEmail(emailAddress, "Wachtwoord opnieuw instellen",
                $""""
                Via deze link kunt U uw wachtwoord opnieuw instellen.
                {link}
                """");

            return Result.Success();
        }

        public async Task<Result> ResetPasswordAsync(string emailAddress, string token, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(emailAddress);

            if (user == null)
            {
                return Result.Failure(new List<string> { "User is not found with the given emailaddress." });
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            return result.ToApplicationResult();
        }
    }
}
