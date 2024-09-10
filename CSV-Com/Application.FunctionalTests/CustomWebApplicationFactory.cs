using System.Data.Common;
using Application.Common.Interfaces.Authentication;
using Infrastructure.Data.Authentication;
using Infrastructure.Data.CVS;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace Application.FunctionalTests
{


    public class CustomWebApplicationFactory(DbConnection connectionCvs, DbConnection connectionAuthentication) : WebApplicationFactory<Program>
    {
        private readonly DbConnection _connectionCvs = connectionCvs;
        private readonly DbConnection _connectionAuthentication = connectionAuthentication;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services
                    .RemoveAll<IUser>()
                    .AddTransient(provider => Mock.Of<IUser>(s => s.CurrentUserId == Testing.GetCurrentUserId()));

                // CVS
                services
                    .RemoveAll<DbContextOptions<CVSDbContext>>()
                    .AddDbContext<CVSDbContext>((sp, options) =>
                    {
                        options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                        options.UseMySql(_connectionCvs, MySqlServerVersion.LatestSupportedServerVersion);
                    });

                // Authentication
                services
                    .RemoveAll<DbContextOptions<AuthenticationDbContext>>()
                    .AddDbContext<AuthenticationDbContext>((sp, options) =>
                    {
                        options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                        options.UseMySql(_connectionAuthentication, MySqlServerVersion.LatestSupportedServerVersion);
                    });
            });
        }
    }
}
