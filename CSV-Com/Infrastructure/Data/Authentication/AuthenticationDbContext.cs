using Infrastructure.Data.Authentication.Configuration;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Authentication
{
    public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : IdentityDbContext<
            AuthenticationUser,
            AuthenticationRole,
            string,
            AuthenticationUserClaim,
            AuthenticationUserRole,
            AuthenticationUserLogin,
            AuthenticationRoleClaim,
            AuthenticationUserToken>(options), IAuthenticationDbContext
    {
        public DbSet<AuthenticationUser> Users { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<TwoFactorPendingToken> TwoFactorPendingTokens { get; set; }

        public DbSet<TemporaryPasswordToken> TemporaryPasswordTokens { get; set; }

        public DbSet<AuthenticationUserRole> UserRoles { get; set; }

        public DbSet<AuthenticationRole> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var configurations = GetType().Assembly.GetTypes()
                .Where(type => type.GetInterfaces().Any(interfaceType =>
                    interfaceType == typeof(IAuthenticationEntityTypeConfiguration)))
                .Select(type => Activator.CreateInstance(type) as IAuthenticationEntityTypeConfiguration);

            foreach (var configuration in configurations)
            {
                configuration!.Configure(builder);
            }
        }
    }
}
