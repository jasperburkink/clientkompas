using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Authentication
{
    public interface IAuthenticationDbContext
    {
        DbSet<AuthenticationUser> Users { get; set; }

        DbSet<AuthenticationUserRole> UserRoles { get; set; }

        DbSet<AuthenticationRole> Roles { get; set; }

        DbSet<RefreshToken> RefreshTokens { get; set; }

        DbSet<TwoFactorPendingToken> TwoFactorPendingTokens { get; set; }

        DbSet<TemporaryPasswordToken> TemporaryPasswordTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
