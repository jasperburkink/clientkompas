﻿using Application.Common.Models;
using Domain.Authentication.Domain;

namespace Application.Common.Interfaces.Authentication
{
    public interface IIdentityService
    {
        Task<string?> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password, int cvsUserId);

        Task<Result> DeleteUserAsync(string userId);

        Task<LoggedInResult> LoginAsync(string userName, string password);

        Task LogoutAsync();

        Task<IList<string>> GetRolesAsync(string userId);

        Task<bool> UserExistsAsync(string userId);

        Task<IAuthenticationUser> GetUserAsync(string userId);

        Task<Result> SendResetPasswordEmailAsync(string emailAddress);

        Task<Result> ResetPasswordAsync(string emailAddress, string token, string newPassword);

        Task<string> Get2FATokenAsync(string userId);

        Task<LoggedInResult> Login2FAAsync(string userId, string token);

        Task<int?> GetCurrentLoggedInUserId();

        Task<Result> AddUserToRoleAsync(string userId, string role);

        Task<IList<string>> GetUserRolesAsync(string userId);

        Task RemoveUserAsync(string userId);

        Task<IList<string>> GetAvailableUserRolesAsync();

        Task UpdateUserAsync(IAuthenticationUser user);

        Task<IList<IAuthenticationUser>> GetUsersInRolesAsync(string role, params string[] roles);
    }
}
