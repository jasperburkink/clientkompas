using Domain.Authentication.Domain;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Authentication
{
    public interface IAuthenticationDbContext
    {
        DbSet<AuthenticationUser> AuthenticationUsers { get; set; }

        DbSet<RefreshToken> RefreshTokens { get; set; }

        DbSet<TwoFactorPendingToken> TwoFactorPendingTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
