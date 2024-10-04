using Domain.Authentication.Domain;

namespace Application.Common.Interfaces.Authentication
{
    public interface IRefreshTokenService
    {
        Task<string> GenerateRefreshTokenAsync(AuthenticationUser user);

        Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken);

        Task RevokeRefreshTokenAsync(string userId, string refreshToken);

        Task<IRefreshToken?> GetRefreshTokenAsync(string refreshToken)
    }
}
