using Application.Common.Interfaces.CVS;
using Infrastructure.Persistence.CVS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130 // Namespace does not match folder structure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStringAuthentication = configuration.GetValue<string>("ConnectionStrings:AuthenticationConnectionString");
            var connectionStringCVS = configuration.GetValue<string>("ConnectionStrings:CVSConnectionString");

            var serverVersion = MySqlServerVersion.LatestSupportedServerVersion;

            // TODO: in memory database when debugging or configuration parameter
            //if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            //{
            //    services.AddDbContext<ApplicationDbContext>(options =>
            //        options.UseInMemoryDatabase("CleanArchitectureDb"));
            //}
            services.AddDbContext<CVSDbContext>(
            dbContextOptions => dbContextOptions
                            .UseMySql(connectionStringCVS, serverVersion,
                            mySqlOptions =>
                            {
                                mySqlOptions.MigrationsAssembly(typeof(CVSDbContext).Assembly.FullName); // Migrations in class library
                            })
                            // The following three options help with debugging, but should
                            // be changed or removed for production.
                            .LogTo(Console.WriteLine, LogLevel.Information)
                            .EnableSensitiveDataLogging()
            .EnableDetailedErrors());

            services.AddScoped<CVSDbContextInitialiser>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // TODO: Identity implementation
            //services.AddScoped<AuthenticationDbContextInitialiser>();

            //services
            //    .AddDefaultIdentity<ApplicationUser>()
            //    .AddRoles<IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddIdentityServer()
            //    .AddApiAuthorization<ApplicationUser, AuthenticationDbContext>();

            //services.AddTransient<IIdentityService, IdentityService>();

            //services.AddAuthentication()
            //    .AddIdentityServerJwt();

            //services.AddAuthorization(options =>
            //    options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

            return services;
        }
    }
}
