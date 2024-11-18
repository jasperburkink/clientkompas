using Domain.Authentication.Domain;

namespace Application.Common.Interfaces.Authentication
{
    public interface ITokenService
    {
        Task<string> GenerateTokenAsync(AuthenticationUser user, string tokenType);

        Task<bool> ValidateTokenAsync(string userId, string tokenValue, string tokenType);

        Task RevokeTokenAsync(string userId, string tokenValue, string tokenType);

        Task<IToken?> GetTokenAsync(string tokenValue, string tokenType);

        Task<IList<IToken>> GetValidTokensByUserAsync(string userId, string tokenType);
    }
}
