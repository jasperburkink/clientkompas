using Application.Authentication.Commands.ResetPassword;
using Application.Common.Exceptions;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using TestData;
using TestData.Authentication;

namespace Application.FunctionalTests.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandValidatorTests : BaseTestFixture
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
        public async Task Handle_CorrectFlow_NoValidationErrors()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Test]
        public void Handle_EmailAddressIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                EmailAddress = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_EmailAddressIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                EmailAddress = string.Empty
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_InvalidEmailAddress_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                EmailAddress = "NotAnEmailAddress"
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_PasswordIsNull_ShouldThrowValidationException()
        {
            // Arrange
            string password = null;

            var command = _command with
            {
                NewPassword = password,
                NewPasswordRepeat = password
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_PasswordIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var password = string.Empty;

            var command = _command with
            {
                NewPassword = password,
                NewPasswordRepeat = password
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_PasswordIsToShort_ShouldThrowValidationException()
        {
            // Arrange
            var password = FakerConfiguration.Faker.Random.String2(AuthenticationUserConstants.PASSWORD_MINLENGTH - 1);

            var command = _command with
            {
                NewPassword = password,
                NewPasswordRepeat = password
            };

            // Act
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_PasswordIsToLong_ShouldThrowValidationException()
        {
            // Arrange
            var password = FakerConfiguration.Faker.Random.String2(AuthenticationUserConstants.PASSWORD_MAXLENGTH + 1);

            var command = _command with
            {
                NewPassword = password,
                NewPasswordRepeat = password
            };

            // Act
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_PasswordContainsNoLowerCaseCharacter_ShouldThrowValidationException()
        {
            // Arrange
            var password = Utils.GeneratePassword().ToUpper();

            var command = _command with
            {
                NewPassword = password,
                NewPasswordRepeat = password
            };

            // Act
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_PasswordContainsNoUpperCaseCharacter_ShouldThrowValidationException()
        {
            // Arrange
            var password = Utils.GeneratePassword().ToLower();

            var command = _command with
            {
                NewPassword = password,
                NewPasswordRepeat = password
            };

            // Act
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_PasswordContainsNoNumber_ShouldThrowValidationException()
        {
            // Arrange
            var password = "TestTest!@#$%^&";

            var command = _command with
            {
                NewPassword = password,
                NewPasswordRepeat = password
            };

            // Act
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_PasswordContainsNoSpecialCharacter_ShouldThrowValidationException()
        {
            // Arrange
            var password = "TestTest123456789";

            var command = _command with
            {
                NewPassword = password,
                NewPasswordRepeat = password
            };

            // Act
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_PasswordsDoNotMatch_ShouldThrowValidationException()
        {
            // Arrange
            var password1 = Utils.GeneratePassword();
            var password2 = Utils.GeneratePassword();

            var command = _command with
            {
                NewPassword = password1,
                NewPasswordRepeat = password2
            };

            // Act
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [TearDown]
        public void TearDown() => UseMocks = false;
    }
}
