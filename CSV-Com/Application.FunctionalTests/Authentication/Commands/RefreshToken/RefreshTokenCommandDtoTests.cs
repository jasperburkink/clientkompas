using Application.Authentication.Commands.RefreshToken;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using TestData;
using TestData.Authentication;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandDtoTests : BaseTestFixture
    {
        private AuthenticationUser _authenticationUser;
        private Infrastructure.Identity.RefreshToken _refreshToken;
        private RefreshTokenCommand _command;

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
        public async Task Handle_CorrectFlow_SuccessShouldBeTrue()
        {
            // Arrange
            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken);

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldReturnRefreshToken()
        {
            // Arrange
            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken);

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.RefreshToken.Should().NotBeNull().And.NotBeEmpty();
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldReturnBearerToken()
        {
            // Arrange
            await AddAsync<Infrastructure.Identity.RefreshToken, AuthenticationDbContext>(_refreshToken);

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.BearerToken.Should().NotBeNull().And.NotBeEmpty();
        }
    }
}
