using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.CVS;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Data.CVS;
using Infrastructure.Data.Interceptor;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionStringAuthentication = configuration.GetValue<string>("ConnectionStrings:AuthenticationConnectionString");
            Guard.Against.Null(connectionStringAuthentication, message: "Connection string 'AuthenticationConnectionString' not found.");

            var connectionStringCVS = configuration.GetValue<string>("ConnectionStrings:CVSConnectionString");
            Guard.Against.Null(connectionStringCVS, message: "Connection string 'CVSConnectionString' not found.");

            services.AddScoped<AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>(sp => sp.GetRequiredService<AuditableEntityInterceptor>());

            var serverVersion = MySqlServerVersion.LatestSupportedServerVersion;

            // CVS
            services.AddDbContext<CVSDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

                options.UseMySql(connectionStringCVS, serverVersion,
                            mySqlOptions =>
                            {
                                mySqlOptions.MigrationsAssembly(typeof(CVSDbContext).Assembly.FullName); // Migrations in class library
#if DEBUG
                                mySqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
                                    maxRetryDelay: TimeSpan.FromSeconds(10),
                                    errorNumbersToAdd: null);
#endif
                            })
                            .LogTo(Console.WriteLine, LogLevel.Information)
                            .EnableSensitiveDataLogging();
            });
            //services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<CVSDbContextInitialiser>();

            // Authentication
            services.AddDbContext<AuthenticationDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

                options.UseMySql(connectionStringAuthentication, serverVersion,
                mySqlOptions =>
                {
                    mySqlOptions.MigrationsAssembly(typeof(AuthenticationDbContext).Assembly.FullName); // Migrations in class library
#if DEBUG
                    mySqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null);
#endif
                })
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging();
            });

            services.AddScoped<AuthenticationDbContextInitialiser>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services
                .AddDefaultIdentity<AuthenticationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AuthenticationDbContext>();

            //services.AddIdentityServer()
            //    .AddApiAuthorization<ApplicationUser, AuthenticationDbContext>();

            services.AddTransient<IDateTime, DateTimeWrapper>();
            services.AddTransient<IIdentityService, IdentityService>();

            //services.AddAuthentication()
            //    .AddIdentityServerJwt();

            services.AddAuthorization(options =>
                    options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

            return services;
        }
    }
}
