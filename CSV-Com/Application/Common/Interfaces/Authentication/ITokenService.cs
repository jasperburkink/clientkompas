using Domain.Authentication.Domain;

namespace Application.Common.Interfaces.Authentication
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(IAuthenticationUser user, string tokenType);

        Task<bool> ValidateTokenAsync(string userId, string tokenValue, string tokenType);

        Task RevokeTokenAsync(string userId, string tokenValue, string tokenType);

        Task<IAuthenticationToken?> GetTokenAsync(string tokenValue, string tokenType);

        Task<IList<IAuthenticationToken>> GetValidTokensByUserAsync(string userId, string tokenType);
    }
}
