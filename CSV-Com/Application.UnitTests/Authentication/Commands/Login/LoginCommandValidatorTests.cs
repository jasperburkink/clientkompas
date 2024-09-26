using Application.Authentication.Commands.Login;
using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Domain.Authentication.Constants;
using FluentValidation.TestHelper;
using Moq;
using TestData;

namespace Application.UnitTests.Authentication.Commands.Login
{
    public class LoginCommandValidatorTests
    {
        private readonly LoginCommandValidator _validator;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;
        private const string VALID_PASSWORD = "TestPassword1!";

        public LoginCommandValidatorTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();
            _resourceMessageProviderMock
                .Setup(m => m.GetMessage(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns((Type type, string key, object[] args) =>
                    $"Mock validation message for Type: {type.Name}, Key: {key}, Args: {string.Join(", ", args ?? [])}");

            _validator = new LoginCommandValidator(_unitOfWorkMock.Object, _resourceMessageProviderMock.Object);
        }

        [Fact]
        public async Task Validate_ValidCommand_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new LoginCommand
            {
                UserName = "Test",
                Password = VALID_PASSWORD
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_UserNameIsLongerThanAllowed_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand
            {
                UserName = FakerConfiguration.Faker.Random.String2(AuthenticationUserConstants.USERNAME_MAXLENGTH + 1),
                Password = VALID_PASSWORD
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.UserName);
        }

        [Fact]
        public async Task Validate_UserNameIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand
            {
                UserName = null,
                Password = VALID_PASSWORD
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.UserName);
        }

        [Fact]
        public async Task Validate_UserNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand
            {
                UserName = string.Empty,
                Password = VALID_PASSWORD
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.UserName);
        }

        [Fact]
        public async Task Validate_PasswordIsLongerThanAllowed_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand
            {
                UserName = "Test",
                Password = FakerConfiguration.Faker.Random.String2(AuthenticationUserConstants.PASSWORD_MAXLENGTH + 1)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(lc => lc.Password);
        }

        [Fact]
        public async Task Validate_PasswordIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand
            {
                UserName = "Test",
                Password = null
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.Password);
        }

        [Fact]
        public async Task Validate_PasswordIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand
            {
                UserName = "Test",
                Password = string.Empty
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.Password);
        }

        [Fact]
        public async Task Validate_PasswordIsTooShort_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand
            {
                UserName = "Test",
                Password = FakerConfiguration.Faker.Random.String2(AuthenticationUserConstants.PASSWORD_MINLENGTH - 1)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.Password);
        }

        [Fact]
        public async Task Validate_PasswordDoesNotContainLowercaseCharacter_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand
            {
                UserName = "Test",
                Password = "TESTDERTEST1!"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.Password);
        }

        [Fact]
        public async Task Validate_PasswordDoesNotContainUppercaseCharacter_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand
            {
                UserName = "Test",
                Password = "testtest1!"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.Password);
        }

        [Fact]
        public async Task Validate_PasswordDoesNotContainNumber_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand
            {
                UserName = "Test",
                Password = "TestTest!?.,"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.Password);
        }

        [Fact]
        public async Task Validate_PasswordDoesNotContainSpecialCharacter_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LoginCommand
            {
                UserName = "Test",
                Password = "TestTest1234"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cp => cp.Password);
        }
    }
}
