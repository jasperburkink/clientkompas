using Application.Authentication.Commands.Logout;
using Application.Common.Exceptions;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using TestData;
using TestData.Authentication;

namespace Application.FunctionalTests.Authentication.Commands.Logout
{
    public class LogoutCommandValidatorTests : BaseTestFixture
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
        public async Task Validate_SuccessFlow_NoValidationErrors()
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
        public async Task Validate_RefreshTokenIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LogoutCommand
            {
                RefreshToken = null
            };

            // Act
            var act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }


        [Test]
        public async Task Validate_RefreshTokenIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LogoutCommand
            {
                RefreshToken = string.Empty
            };

            // Act
            var act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
