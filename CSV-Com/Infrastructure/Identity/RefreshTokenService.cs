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
        private readonly IAuthenticationDbContext _authenticationDbContext;
        private readonly IHasher _hasher;

        public const string REFRESH_SECRET_KEY = "thisisarefreshsecretkey@123"; // TODO: Move to the keyvault

        public RefreshTokenService(IAuthenticationDbContext authenticationDbContext, IHasher hasher)
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
            var randomNumberBase64String = Convert.ToBase64String(randomNumber);

            var salt = _hasher.GenerateSalt();
            var refreshToken = _hasher.HashString(randomNumberBase64String, salt);

            var tokenEntry = new RefreshToken
            {
                Name = $"{RefreshTokenConstants.NAME}_{DateTime.UtcNow.Ticks}",
                LoginProvider = RefreshTokenConstants.LOGINPROVIDER,
                Value = refreshToken,
                UserId = user.Id,
                ExpiresAt = DateTime.UtcNow.Add(RefreshTokenConstants.TOKEN_TIMEOUT),
                CreatedAt = DateTime.UtcNow,
                IsUsed = true,
                IsRevoked = false
            };

            await _authenticationDbContext.RefreshTokens.AddAsync(tokenEntry);
            await _authenticationDbContext.SaveChangesAsync();

            foreach (var oldToken in _authenticationDbContext.RefreshTokens.Where(rt =>
            rt.UserId == user.Id
            && !rt.IsRevoked
            && !string.IsNullOrEmpty(rt.Value)
            && rt.Value != refreshToken))
            {
                await RevokeRefreshTokenAsync(user.Id, oldToken.Value!);
            }

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
                token.IsUsed = false;
                await _authenticationDbContext.SaveChangesAsync();
            }
        }

        public async Task<IRefreshToken?> GetRefreshTokenAsync(string refreshToken)
        {
            return await _authenticationDbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Value == refreshToken);
        }
    }
}
