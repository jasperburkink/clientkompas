using System.Data.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Data.CVS;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using TestData;
using TestData.Authentication;

namespace Application.FunctionalTests
{
    public class CustomWebApplicationFactoryWithMocks(DbConnection connectionCvs, DbConnection connectionAuthentication) : WebApplicationFactory<Program>
    {
        private readonly DbConnection _connectionCvs = connectionCvs;
        private readonly DbConnection _connectionAuthentication = connectionAuthentication;
        private static readonly ITestDataGenerator<AuthenticationUser> s_testDataGeneratorAuthenticationUser = new AuthenticationUserDataGenerator();
        public static readonly AuthenticationUser AuthenticationUser = s_testDataGeneratorAuthenticationUser.Create();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services
                    .RemoveAll<IUser>()
                    .AddTransient(provider => Mock.Of<IUser>(s => s.CurrentUserId == GetCurrentUserId()));

                // Mock 
                AuthenticationUser.TwoFactorEnabled = true;
                AddAsync<IAuthenticationUser, AuthenticationDbContext>(AuthenticationUser).GetAwaiter().GetResult();
                var mockIdentityService = new Mock<IIdentityService>();
                mockIdentityService.Setup(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new LoggedInResult(true, AuthenticationUser, [Roles.Coach]));
                mockIdentityService.Setup(s => s.LogoutAsync()).Returns(Task.CompletedTask);
                mockIdentityService.Setup(s => s.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Result.Success);
                mockIdentityService.Setup(s => s.Login2FAAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new LoggedInResult(true, AuthenticationUser, [Roles.Coach]));
                mockIdentityService.Setup(s => s.Get2FATokenAsync(It.IsAny<string>())).ReturnsAsync("2FA");
                services.RemoveAll<IIdentityService>();
                services.AddSingleton(mockIdentityService.Object);

                var emailServiceMock = new Mock<IEmailService>();
                emailServiceMock.Setup(mock => mock.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));


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
