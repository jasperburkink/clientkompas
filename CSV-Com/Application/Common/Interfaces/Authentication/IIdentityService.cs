using Application.Common.Models;
using Domain.Authentication.Domain;

namespace Application.Common.Interfaces.Authentication
{
    public interface IIdentityService
    {
        Task<string?> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

        Task<Result> DeleteUserAsync(string userId);

        Task<LoggedInResult> LoginAsync(string userName, string password);

        Task LogoutAsync();

        Task<IList<string>> GetRolesAsync(string userId);

        Task<bool> UserExistsAsync(string userId);

        Task<AuthenticationUser> GetUserAsync(string userId);
    }
}
