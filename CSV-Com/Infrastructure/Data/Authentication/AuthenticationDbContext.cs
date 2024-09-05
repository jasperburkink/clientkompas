using Domain.Authentication.Domain;
using Duende.IdentityServer.EntityFramework.Options;
using Infrastructure.Data.Interceptor;
using Infrastructure.Identity;
using Infrastructure.Persistence.Authentication.Configuration;
using MediatR;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data.Authentication
{
    public class AuthenticationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        private readonly IMediator _mediator;
        private readonly AuditableEntityInterceptor _auditableEntitySaveChangesInterceptor;

        public DbSet<AuthenticationUser> AuthenticationUsers { get; set; }

        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options,
        IOptions<OperationalStoreOptions> operationalStoreOptions,
        IMediator mediator,
        AuditableEntityInterceptor auditableEntitySaveChangesInterceptor) : base(options, operationalStoreOptions)
        {
            _mediator = mediator;
            _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
        }

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _mediator.DispatchDomainEvents(this);

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
