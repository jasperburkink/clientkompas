using Application.Common.Interfaces.Authentication;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Authentication;
using Infrastructure.Persistence.CVS;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;

namespace Application.IntegrationTests
{
    internal class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private string CurrentUserId { get; init; }

        public CustomWebApplicationFactory(string currentUserId)
        {
            CurrentUserId = currentUserId;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                var integrationConfig = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

                configurationBuilder.AddConfiguration(integrationConfig);
            });

            builder.ConfigureServices((builder, services) =>
            {
                services
                    .Remove<ICurrentUserService>()
                    .AddTransient(provider => Mock.Of<ICurrentUserService>(s =>
                        s.UserId == CurrentUserId));

                services
                    .Remove<DbContextOptions<AuthenticationDbContext>>()
                    .AddDbContext<AuthenticationDbContext>((sp, options) =>
                        options.UseMySql(builder.Configuration.GetConnectionString("AuthenticationConnectionString"), MySqlServerVersion.LatestSupportedServerVersion,
                            builder => builder.MigrationsAssembly(typeof(AuthenticationDbContext).Assembly.FullName)));

                services
                    .Remove<DbContextOptions<CVSDbContext>>()
                    .AddDbContext<CVSDbContext>((sp, options) =>
                        options.UseMySql(builder.Configuration.GetConnectionString("CVSConnectionString"), MySqlServerVersion.LatestSupportedServerVersion,
                            builder => builder.MigrationsAssembly(typeof(CVSDbContext).Assembly.FullName)));
            });
        }
    }
}
