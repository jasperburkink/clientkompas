using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.CVS;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Data.CVS;
using Infrastructure.Data.Interceptor;
using Infrastructure.Identity;
using Infrastructure.Services;
using Infrastructure.Services.Menu;
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
            services.AddScoped<IAuthenticationDbContext, AuthenticationDbContext>();
            services.AddScoped<AuthenticationDbContextInitialiser>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services
                .AddDefaultIdentity<AuthenticationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AuthenticationDbContext>();

            services.AddHttpContextAccessor();

            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IMenuService, MenuService>();
            services.AddSingleton<IDateTime, DateTimeWrapper>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IBearerTokenService, BearerTokenService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordHasher<AuthenticationUser>, Argon2PasswordHasher>();
            services.AddScoped<IHasher, Argon2Hasher>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPasswordService, PasswordService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.LicenceManagement, policy =>
                {
                    policy.RequireRole(Roles.SystemOwner);
                });

                options.AddPolicy(Policies.LicenceRead, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.UserManagement, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator]);
                });

                options.AddPolicy(Policies.UserRead, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.ClientManagement, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.ClientRead, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.OrganizationManagement, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.OrganizationRead, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.DriversLicenceManagement, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator]);
                });

                options.AddPolicy(Policies.DriversLicenceRead, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.MaritalStatusManagement, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator]);
                });

                options.AddPolicy(Policies.MaritalStatusRead, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.DiagnosisManagement, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator]);
                });

                options.AddPolicy(Policies.DiagnosisRead, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.BenefitFormManagement, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator]);
                });

                options.AddPolicy(Policies.BenefitFormRead, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.TimeRegistrationManagement, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.TimeRegistrationRead, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.CoachingProgramManagement, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.CoachingProgramRead, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.AssignAdministrator, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee]);
                });

                options.AddPolicy(Policies.AnonymizeCoach, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator]);
                });

                options.AddPolicy(Policies.EditCoach, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.GenerateRaport, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach]);
                });

                options.AddPolicy(Policies.DeactiveClient, policy =>
                {
                    policy.RequireRole([Roles.SystemOwner, Roles.Licensee, Roles.Administrator]);
                });
            });

            return services;
        }
    }
}
