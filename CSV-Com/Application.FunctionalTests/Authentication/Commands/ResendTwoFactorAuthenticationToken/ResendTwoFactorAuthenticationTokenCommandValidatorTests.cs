using Application.Authentication.Commands.ResendTwoFactorAuthenticationToken;
using Application.Common.Exceptions;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
using TestData;
using TestData.Authentication;

namespace Application.FunctionalTests.Authentication.Commands.ResendTwoFactorAuthenticationToken
{
    public class ResendTwoFactorAuthenticationTokenCommandValidatorTests : BaseTestFixture
    {
        private ResendTwoFactorAuthenticationTokenCommand _command;
        private AuthenticationUser _authenticationUser;

        [SetUp]
        public async Task SetUp()
        {
            ITestDataGenerator<IAuthenticationUser> testDataGeneratorAuthenticationUser = new AuthenticationUserDataGenerator();
            _authenticationUser = testDataGeneratorAuthenticationUser.Create() as AuthenticationUser;

            var initialPassword = Utils.GeneratePassword();
            var id = await CreateUserAsync(_authenticationUser.Email!, initialPassword);

            _authenticationUser.Id = id;

            var token = new TwoFactorPendingToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(1),
                IsRevoked = false,
                IsUsed = false,
                UserId = _authenticationUser.Id,
                LoginProvider = "Test",
                Name = "TestTwoFactorPendingToken",
                Value = "Test"
            };
            await AddAsync<TwoFactorPendingToken, AuthenticationDbContext>(token);

            _command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = _authenticationUser.Id,
                TwoFactorPendingToken = token.Value
            };
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldNotThrowValidationException()
        {
            // Act
            var act = async () => await SendAsync(_command);

            // Assert
            await act.Should().NotThrowAsync<ValidationException>();
        }

        [Test]
        public async Task Handle_UserIdIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                UserId = null
            };

            // Act
            var act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task Handle_UserIdIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                UserId = string.Empty
            };

            // Act
            var act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task Handle_TwoFactorPendingTokenIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                TwoFactorPendingToken = null
            };

            // Act
            var act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task Handle_TwoFactorPendingTokenIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                TwoFactorPendingToken = string.Empty
            };

            // Act
            var act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
