using Application.Authentication.Commands.ResetPassword;
using Domain.Authentication.Domain;
using Infrastructure.Identity;
using TestData;
using TestData.Authentication;

namespace Application.FunctionalTests.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandTests : BaseTestFixture
    {
        private AuthenticationUser _authenticationUser;
        private ResetPasswordCommand _command;

        [SetUp]
        public async Task SetUp()
        {
            UseMocks = true;

            ITestDataGenerator<IAuthenticationUser> testDataGeneratorAuthenticationUser = new AuthenticationUserDataGenerator();
            _authenticationUser = testDataGeneratorAuthenticationUser.Create() as AuthenticationUser;

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
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task Handle_EmailAddressIsNull_SuccessIsFalse()
        {
            // Arrange
            var command = _command with
            {
                EmailAddress = null
            };

            var handler = new ResetPasswordCommandHandler(Testing.IdentityService);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
        }

        [Test]
        public async Task Handle_TokenIsNull_SuccessIsFalse()
        {
            // Arrange
            var command = _command with
            {
                Token = null
            };

            var handler = new ResetPasswordCommandHandler(Testing.IdentityService);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
        }

        [Test]
        public async Task Handle_NewPasswordIsNull_SuccessIsFalse()
        {
            // Arrange
            var command = _command with
            {
                NewPassword = null
            };

            var handler = new ResetPasswordCommandHandler(Testing.IdentityService);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
        }


        [TearDown]
        public void TearDown() => UseMocks = false;
    }
}
