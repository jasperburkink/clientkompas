using Domain.Authentication.Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Authentication
{
    public interface IAuthenticationDbContext
    {
        DbSet<AuthenticationUser> AuthenticationUsers { get; set; }

        DbSet<IRefreshToken> RefreshTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
