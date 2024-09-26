using Application.Authentication.Commands.Login;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using FluentValidation;
using TestData;
using TestData.Authentication;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Authentication.Commands
{
    public class LoginCommandValidatorTests : BaseTestFixture
    {
        private string _password;
        private LoginCommand _command;
        private ITestDataGenerator<AuthenticationUser> _testDataGeneratorAuthenticationUser;

        [SetUp]
        public async Task SetUp()
        {
            _testDataGeneratorAuthenticationUser = new AuthenticationUserDataGenerator();
            var authenticationUser = _testDataGeneratorAuthenticationUser.Create();

            _password = PasswordGenerator.GenerateSecurePassword(16);

            await RunAsUserAsync(authenticationUser.UserName, _password, Roles.Coach);

            _command = new LoginCommand
            {
                UserName = authenticationUser.UserName,
                Password = _password
            };
        }

        [Ignore("Logging in via tests is not working yet.")]
        //[Test]
        public void Handle_CorrectFlow_NoValidationExcceptions()
        {
            // Act & Assert
            Assert.DoesNotThrowAsync(() => SendAsync(_command));
        }

        [Ignore("Logging in via tests is not working yet.")]
        //[Test]
        public void Handle_UserNameIsLongerThanAllowed_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                UserName = FakerConfiguration.Faker.Random.String2(AuthenticationUserConstants.USERNAME_MAXLENGTH + 1)
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Ignore("Logging in via tests is not working yet.")]
        //[Test]
        public void Handle_UserNameIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                UserName = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Ignore("Logging in via tests is not working yet.")]
        //[Test]
        public void Handle_UserNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                UserName = string.Empty
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Ignore("Logging in via tests is not working yet.")]
        //[Test]
        public void Handle_PasswordIsLongerThanAllowed_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                Password = FakerConfiguration.Faker.Random.String2(AuthenticationUserConstants.PASSWORD_MAXLENGTH + 1)
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Ignore("Logging in via tests is not working yet.")]
        //[Test]
        public void Handle_PasswordIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                Password = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Ignore("Logging in via tests is not working yet.")]
        //[Test]
        public void Handle_PasswordIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                Password = string.Empty
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Ignore("Logging in via tests is not working yet.")]
        //[Test]
        public void Handle_PasswordIsShorterThanAllowed_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                Password = FakerConfiguration.Faker.Random.String2(AuthenticationUserConstants.PASSWORD_MINLENGTH - 1)
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Ignore("Logging in via tests is not working yet.")]
        //[Test]
        public void Handle_PasswordDoesNotContainLowercaseCharacter_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                Password = "TESTDERTEST1!"
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Ignore("Logging in via tests is not working yet.")]
        //[Test]
        public void Handle_PasswordDoesNotContainUppercaseCharacter_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                Password = "testtest1!"
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        public void Handle_PasswordDoesNotContainNumber_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                Password = "TestTest!?.,"
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        public void Handle_PasswordDoesNotSpecialCharacter_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                Password = "TestTest1234"
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }
    }
}
