using Application.Authentication.Commands.Logout;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using TestData;
using TestData.Authentication;

namespace Application.FunctionalTests.Authentication.Commands.Logout
{
    public class LogoutCommandTests : BaseTestFixture
    {
        private AuthenticationUser _authenticationUser;
        private Infrastructure.Identity.RefreshToken _refreshToken, _refreshToken2, _refreshToken3;
        private LogoutCommand _command;

        [SetUp]
        public async Task SetUp()
        {
            ITestDataGenerator<AuthenticationUser> testDataGeneratorAuthenticationUser = new AuthenticationUserDataGenerator();
            _authenticationUser = testDataGeneratorAuthenticationUser.Create();
            await AddAsync<AuthenticationUser, AuthenticationDbContext>(_authenticationUser);

            _refreshToken = new Infrastructure.Identity.RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                LoginProvider = "Test",
                Name = "Test1",
                UserId = _authenticationUser.Id,
                Value = "TestRefreshToken1"
            };

            _refreshToken2 = new Infrastructure.Identity.RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                LoginProvider = "Test",
                Name = "Test2",
                UserId = _authenticationUser.Id,
                Value = "TestRefreshToken2"
            };

            _refreshToken3 = new Infrastructure.Identity.RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                LoginProvider = "Test",
                Name = "Test3",
                UserId = _authenticationUser.Id,
                Value = "TestRefreshToken3"
            };

            _command = new LogoutCommand
            {
                RefreshToken = _refreshToken.Value
            };
        }

        [Test]
        public async Task Handle_SuccessFlow_SuccessIsTrue()
        {
            // Arrange
            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken);
            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken2);
            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken3);

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task Handle_SuccessFlow_AllUserTokensRevoked()
        {
            // Arrange
            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken);
            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken2);
            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken3);

            // Act
            var result = await SendAsync(_command);
            var tokens = (await GetAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>())
                .Where(rt => rt.UserId == _authenticationUser.Id);

            // Assert
            tokens.Should().AllSatisfy(rt => rt.IsRevoked.Should().BeTrue());
        }
    }
}
