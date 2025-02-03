using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication.Configuration;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Authentication
{
    public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : IdentityDbContext<AuthenticationUser>(options), IAuthenticationDbContext
    {
        public DbSet<AuthenticationUser> AuthenticationUsers { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<TwoFactorPendingToken> TwoFactorPendingTokens { get; set; }

        public DbSet<TemporaryPasswordToken> TemporaryPasswordTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var configurations = GetType().Assembly.GetTypes()
                .Where(type => type.GetInterfaces().Any(interfaceType =>
                    interfaceType == typeof(IAuthenticationEntityTypeConfiguration)))
                .Select(type => Activator.CreateInstance(type) as IAuthenticationEntityTypeConfiguration);

            foreach (var configuration in configurations)
            {
                configuration!.Configure(builder);
            }

            base.OnModelCreating(builder);
        }
    }
}
