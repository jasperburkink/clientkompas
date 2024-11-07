using Application.Authentication.Commands.ResetPassword;
using Domain.Authentication.Domain;
using TestData;
using TestData.Authentication;

namespace Application.FunctionalTests.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandDtoTests : BaseTestFixture
    {
        private AuthenticationUser _authenticationUser;
        private ResetPasswordCommand _command;

        [SetUp]
        public async Task SetUp()
        {
            UseMocks = true;

            ITestDataGenerator<AuthenticationUser> testDataGeneratorAuthenticationUser = new AuthenticationUserDataGenerator();
            _authenticationUser = testDataGeneratorAuthenticationUser.Create();

            var initialPassword = Utils.GeneratePassword();
            await CreateUserAsync(_authenticationUser.Email!, initialPassword);

            var password = Utils.GeneratePassword();

            var token = await GetPasswordResetTokenAsync(_authenticationUser);

            _command = new ResetPasswordCommand
            {
                EmailAddress = _authenticationUser.Email!,
                NewPassword = password,
                NewPasswordRepeat = password,
                Token = token
            };
        }

        [Test]
        public async Task Handle_CorrectFlow_SuccessIsTrue()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task Handle_CorrectFlow_ErrorsIsEmpty()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public async Task Handle_PasswordIsNull_SuccessIsFalse()
        {
            // Arrange
            var command = _command with
            {
                NewPassword = null
            };

            var handler = new ResetPasswordCommandHandler(IdentityService);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().BeFalse();
        }

        [Test]
        public async Task Handle_PasswordIsNull_ErrorsContainError()
        {
            // Arrange
            var command = _command with
            {
                NewPassword = null
            };

            var handler = new ResetPasswordCommandHandler(IdentityService);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Errors.Should().NotBeEmpty();
        }

        [TearDown]
        public void TearDown() => UseMocks = false;
    }
}
