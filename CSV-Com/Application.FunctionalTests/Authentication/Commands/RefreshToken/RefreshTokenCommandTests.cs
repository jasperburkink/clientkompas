using Application.Authentication.Commands.RefreshToken;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
using TestData;
using TestData.Authentication;

namespace Application.FunctionalTests.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandTests : BaseTestFixture
    {
        private AuthenticationUser _authenticationUser;
        private Infrastructure.Identity.RefreshToken _refreshToken;
        private RefreshTokenCommand _command;

        [SetUp]
        public async Task SetUp()
        {
            ITestDataGenerator<IAuthenticationUser> testDataGeneratorAuthenticationUser = new AuthenticationUserDataGenerator();
            _authenticationUser = testDataGeneratorAuthenticationUser.Create() as AuthenticationUser;
            await AddAsync<AuthenticationUser, AuthenticationDbContext>(_authenticationUser);

            _refreshToken = new Infrastructure.Identity.RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                LoginProvider = "Test",
                Name = "Test",
                UserId = _authenticationUser.Id,
                Value = "TestRefreshToken"
            };

            _command = new RefreshTokenCommand
            {
                RefreshToken = _refreshToken.Value
            };
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldReturnTokens()
        {
            // Arrange
            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken);

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.BearerToken.Should().NotBeEmpty();
            result.RefreshToken.Should().NotBeEmpty();
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldReturnNewRefreshToken()
        {
            // Arrange
            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken);

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.RefreshToken.Should().NotBeNull().And.NotBe(_refreshToken.Value);
        }

        [Test]
        public async Task Handle_CorrectFlow_OldRefreshTokenShouldBeUsed()
        {
            // Arrange
            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken);

            // Act
            var result = await SendAsync(_command);
            var refreshToken = await FindAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken.UserId, _refreshToken.LoginProvider, _refreshToken.Name);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            refreshToken.IsUsed.Should().BeTrue();
        }

        [Test]
        public async Task Handle_RefreshTokenNotFound_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken);

            var command = new RefreshTokenCommand
            {
                RefreshToken = "TestToken"
            };

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public async Task Handle_TokenAlreadyUsed_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            _refreshToken.IsUsed = true;

            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken);

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            {
                await SendAsync(_command);
            });
        }

        [Test]
        public async Task Handle_TokenRevoked_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            _refreshToken.IsRevoked = true;

            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken);

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            {
                await SendAsync(_command);
            });
        }

        [Test]
        public async Task Handle_TokenExpired_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            _refreshToken.ExpiresAt = DateTime.UtcNow.AddDays(-1);

            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken);

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            {
                await SendAsync(_command);
            });
        }

        [Test]
        public async Task Handle_TwoRequestsWithSameRefreshToken_SecondRequestShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var command1 = new RefreshTokenCommand
            {
                RefreshToken = _refreshToken.Value
            };

            var command2 = new RefreshTokenCommand
            {
                RefreshToken = _refreshToken.Value
            };

            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken);

            // Act & Assert
            var result = await SendAsync(command1);

            Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
            {
                await SendAsync(command2);
            });
        }
    }
}
