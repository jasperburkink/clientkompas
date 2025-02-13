using System.Security.Cryptography;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity
{
    public class TokenService(IAuthenticationDbContext authenticationDbContext, IHasher hasher) : ITokenService
    {
        public async Task<string> GenerateTokenAsync(AuthenticationUser user, string tokenType)
        {
            ArgumentNullException.ThrowIfNull(user);

            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            var randomNumberBase64String = Convert.ToBase64String(randomNumber);

            var salt = hasher.GenerateSalt();
            var tokenValue = hasher.HashString(randomNumberBase64String, salt);

            IToken tokenEntry = tokenType switch
            {
                "RefreshToken" => tokenEntry = new RefreshToken
                {
                    Name = $"{RefreshTokenConstants.NAME}_{DateTime.UtcNow.Ticks}",
                    LoginProvider = TokenConstants.LOGINPROVIDER,
                    Value = tokenValue,
                    UserId = user.Id,
                    ExpiresAt = DateTime.UtcNow.Add(RefreshTokenConstants.TOKEN_TIMEOUT),
                    CreatedAt = DateTime.UtcNow,
                    IsUsed = false,
                    IsRevoked = false
                },
                "TwoFactorPendingToken" => new TwoFactorPendingToken
                {
                    Name = $"{TwoFactorPendingTokenConstants.NAME}_{DateTime.UtcNow.Ticks}",
                    LoginProvider = TokenConstants.LOGINPROVIDER,
                    Value = tokenValue,
                    UserId = user.Id,
                    ExpiresAt = DateTime.UtcNow.Add(TwoFactorPendingTokenConstants.TOKEN_TIMEOUT),
                    CreatedAt = DateTime.UtcNow,
                    IsUsed = false,
                    IsRevoked = false
                },
                "TemporaryPasswordToken" => new TemporaryPasswordToken
                {
                    Name = $"{TemporaryPasswordTokenConstants.NAME}_{DateTime.UtcNow.Ticks}",
                    LoginProvider = TokenConstants.LOGINPROVIDER,
                    Value = tokenValue,
                    UserId = user.Id,
                    ExpiresAt = DateTime.UtcNow.Add(TemporaryPasswordTokenConstants.TOKEN_TIMEOUT),
                    CreatedAt = DateTime.UtcNow,
                    IsUsed = false,
                    IsRevoked = false
                },
                _ => throw new InvalidOperationException($"{tokenType} is not a valid tokentype")
            };

            if (tokenEntry.GetType() == typeof(RefreshToken))
            {
                await authenticationDbContext.RefreshTokens.AddAsync((RefreshToken)tokenEntry);
                await authenticationDbContext.SaveChangesAsync();

                foreach (var oldToken in authenticationDbContext.RefreshTokens.Where(rt =>
                rt.UserId == user.Id
                && !rt.IsRevoked
                && !rt.IsUsed
                && !string.IsNullOrEmpty(rt.Value)
                && rt.Value != tokenValue))
                {
                    await RevokeTokenAsync(user.Id, oldToken.Value!, tokenType);
                }
            }
            else if (tokenEntry.GetType() == typeof(TwoFactorPendingToken))
            {
                await authenticationDbContext.TwoFactorPendingTokens.AddAsync((TwoFactorPendingToken)tokenEntry);
                await authenticationDbContext.SaveChangesAsync();

                foreach (var oldToken in authenticationDbContext.TwoFactorPendingTokens.Where(rt =>
                rt.UserId == user.Id
                && !rt.IsRevoked
                && !rt.IsUsed
                && !string.IsNullOrEmpty(rt.Value)
                && rt.Value != tokenValue))
                {
                    await RevokeTokenAsync(user.Id, oldToken.Value!, tokenType);
                }
            }
            else if (tokenEntry.GetType() == typeof(TemporaryPasswordToken))
            {
                await authenticationDbContext.TemporaryPasswordTokens.AddAsync((TemporaryPasswordToken)tokenEntry);
                await authenticationDbContext.SaveChangesAsync();

                foreach (var oldToken in authenticationDbContext.TemporaryPasswordTokens.Where(rt =>
                rt.UserId == user.Id
                && !rt.IsRevoked
                && !rt.IsUsed
                && !string.IsNullOrEmpty(rt.Value)
                && rt.Value != tokenValue))
                {
                    await RevokeTokenAsync(user.Id, oldToken.Value!, tokenType);
                }
            }
            else
            {
                throw new ArgumentException("Invalid token type", nameof(tokenType));
            }

            return tokenValue;
        }

        public async Task<bool> ValidateTokenAsync(string userId, string tokenValue, string tokenType)
        {
            ArgumentNullException.ThrowIfNull(userId);
            ArgumentNullException.ThrowIfNull(tokenValue);

            var token = tokenType == "RefreshToken"
                ? await authenticationDbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Value == tokenValue && rt.UserId == userId && !rt.IsUsed && !rt.IsRevoked)
                : tokenType == "TwoFactorPendingToken"
                    ? (IToken?)await authenticationDbContext.TwoFactorPendingTokens.FirstOrDefaultAsync(t => t.Value == tokenValue && t.UserId == userId && !t.IsUsed && !t.IsRevoked)
                    : throw new ArgumentException("Invalid token type", nameof(tokenType));
            if (token == null || token.IsExpired)
            {
                return false;
            }

            token.IsUsed = true;
            await authenticationDbContext.SaveChangesAsync();
            return true;
        }

        public async Task RevokeTokenAsync(string userId, string tokenValue, string tokenType)
        {
            ArgumentNullException.ThrowIfNull(userId);
            ArgumentNullException.ThrowIfNull(tokenValue);

            IToken? token = tokenType switch
            {
                nameof(RefreshToken) => token = await authenticationDbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Value == tokenValue && rt.UserId == userId),
                nameof(TwoFactorPendingToken) => await authenticationDbContext.TwoFactorPendingTokens.FirstOrDefaultAsync(t => t.Value == tokenValue && t.UserId == userId),
                nameof(TemporaryPasswordToken) => await authenticationDbContext.TemporaryPasswordTokens.FirstOrDefaultAsync(t => t.Value == tokenValue && t.UserId == userId),
                _ => throw new ArgumentException("Invalid token type", nameof(tokenType))
            };

            if (token != null)
            {
                token.IsRevoked = true;
                await authenticationDbContext.SaveChangesAsync();
            }
        }

        public async Task<IToken?> GetTokenAsync(string tokenValue, string tokenType)
        {
            if (tokenType == "RefreshToken")
            {
                return await authenticationDbContext.RefreshTokens
                    .FirstOrDefaultAsync(rt => rt.Value == tokenValue);
            }
            else if (tokenType == "TwoFactorPendingToken")
            {
                return await authenticationDbContext.TwoFactorPendingTokens
                    .FirstOrDefaultAsync(t => t.Value == tokenValue);
            }
            else
            {
                throw new ArgumentException("Invalid token type", nameof(tokenType));
            }
        }

        public async Task<IList<IToken>> GetValidTokensByUserAsync(string userId, string tokenType)
        {
            if (tokenType == "RefreshToken")
            {
                return await authenticationDbContext.RefreshTokens
                    .Where(rt => rt.UserId == userId && !rt.IsUsed && !rt.IsRevoked)
                    .ToListAsync<IToken>();
            }
            else if (tokenType == "TwoFactorPendingToken")
            {
                return await authenticationDbContext.TwoFactorPendingTokens
                    .Where(t => t.UserId == userId && !t.IsUsed && !t.IsRevoked)
                    .ToListAsync<IToken>();
            }
            else
            {
                throw new ArgumentException("Invalid token type", nameof(tokenType));
            }
        }
    }
}
