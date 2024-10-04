using System.Security.Cryptography;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AuthenticationDbContext _authenticationDbContext;
        private readonly IHasher _hasher;

        public const string REFRESH_SECRET_KEY = "thisisarefreshsecretkey@123"; // TODO: Move to the keyvault

        public RefreshTokenService(AuthenticationDbContext authenticationDbContext, IHasher hasher)
        {
            _authenticationDbContext = authenticationDbContext;
            _hasher = hasher;
        }

        public async Task<string> GenerateRefreshTokenAsync(AuthenticationUser user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            var refreshToken = Convert.ToBase64String(randomNumber);

            var salt = _hasher.GenerateSalt();
            var hashedToken = _hasher.HashString(refreshToken, salt);

            var tokenEntry = new RefreshToken
            {
                Name = RefreshTokenConstants.NAME,
                Value = hashedToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.Add(RefreshTokenConstants.TOKEN_TIMEOUT),
                CreatedAt = DateTime.UtcNow,
                IsUsed = false,
                IsRevoked = false,
                Salt = salt
            };

            _authenticationDbContext.RefreshTokens.Add(tokenEntry);
            await _authenticationDbContext.SaveChangesAsync();

            // TODO: Revoke old tokens for this user

            return refreshToken;
        }

        public async Task<bool> ValidateRefreshTokenAsync(string userId, string refreshToken)
        {
            ArgumentNullException.ThrowIfNull(userId);
            ArgumentNullException.ThrowIfNull(refreshToken);

            var token = await _authenticationDbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Value == refreshToken && rt.UserId == userId && !rt.IsUsed && !rt.IsRevoked);

            if (token == null || token.ExpiresAt < DateTime.UtcNow)
            {
                return false;
            }

            token.IsUsed = true;
            await _authenticationDbContext.SaveChangesAsync();

            return true;
        }

        public async Task RevokeRefreshTokenAsync(string userId, string refreshToken)
        {
            ArgumentNullException.ThrowIfNull(userId);
            ArgumentNullException.ThrowIfNull(refreshToken);

            var token = await _authenticationDbContext.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Value == refreshToken && rt.UserId == userId);

            if (token != null)
            {
                token.IsRevoked = true;
                await _authenticationDbContext.SaveChangesAsync();
            }
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken)
        {
            return await _authenticationDbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Value == refreshToken);
        }
    }
}
