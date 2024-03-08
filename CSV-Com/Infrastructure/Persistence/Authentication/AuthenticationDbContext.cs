using Domain.Authentication.Domain;
using Infrastructure.Persistence.Authentication.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Authentication
{
    public class AuthenticationDbContext : DbContext
    {
        public DbSet<AuthenticationUser> AuthenticationUsers { get; set; }

        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options) { }

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
