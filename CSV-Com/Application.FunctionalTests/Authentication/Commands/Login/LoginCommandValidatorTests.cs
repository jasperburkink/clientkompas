using Application.Authentication.Commands.Login;
using Application.Common.Exceptions;
using Domain.Authentication.Constants;
using TestData;

namespace Application.FunctionalTests.Authentication.Commands.Login
{
    public class LoginCommandValidatorTests : BaseTestFixture
    {
        private string _password;
        private LoginCommand _command;

        [SetUp]
        public async Task SetUp()
        {
            UseMocks = true;

            _password = PasswordGenerator.GenerateSecurePassword(16);

            _command = new LoginCommand
            {
                UserName = CustomWebApplicationFactoryWithMocks.AuthenticationUser.UserName,
                Password = _password
            };
        }

        [Test]
        public async Task Handle_CorrectFlow_NoValidationExcceptions()
        {
            // Act & Assert
            Func<Task<LoginCommandDto>> act = async () => await SendAsync(_command);

            await act.Should().NotThrowAsync<ValidationException>();
        }

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [Test]
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

        [TearDown]
        public void TearDown() => UseMocks = false;
    }
}
