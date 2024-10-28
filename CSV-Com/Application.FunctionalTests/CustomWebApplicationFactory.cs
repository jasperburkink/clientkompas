using System.Data.Common;
using Application.Common.Interfaces.Authentication;
using Infrastructure.Data.Authentication;
using Infrastructure.Data.CVS;
using Microsoft.AspNetCore.Builder;
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
                    .AddTransient(provider => Mock.Of<IUser>(s => s.CurrentUserId == GetCurrentUserId()));

                // Mock 
                var mockIdentityService = new Mock<IIdentityService>();
                mockIdentityService.Setup(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Common.Models.LoggedInResult(true));
                mockIdentityService.Setup(s => s.LogoutAsync()).Returns(Task.CompletedTask);
                services.RemoveAll<IIdentityService>();
                services.AddSingleton(mockIdentityService.Object);

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

            builder.Configure(app =>
            {
                // Configure middleware for testing
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
            });
        }
    }
}
