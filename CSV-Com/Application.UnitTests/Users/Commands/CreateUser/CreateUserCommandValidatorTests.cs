using System.Linq.Expressions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Users.Commands.CreateUser;
using FluentValidation.TestHelper;
using Moq;
using TestData;
using TestData.User.Commands;

namespace Application.UnitTests.Users.Commands.CreateUser
{
    public class CreateUserCommandValidatorTests
    {
        private readonly CreateUserCommandValidator _validator;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;
        private readonly CreateUserCommand _command;

        public CreateUserCommandValidatorTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uw => uw.UserRepository.ExistsAsync(
                It.IsAny<Expression<Func<Domain.CVS.Domain.User, bool>>>(),
                    It.IsAny<CancellationToken>()
                )).ReturnsAsync(false); // Simuleer geen bestaand gebruiker voor email of telefoon.

            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();
            _resourceMessageProviderMock
                .Setup(m => m.GetMessage(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns((Type type, string key, object[] args) =>
                    $"Mock validation message for Type: {type.Name}, Key: {key}, Args: {string.Join(", ", args ?? [])}");

            _validator = new CreateUserCommandValidator(_unitOfWorkMock.Object, _resourceMessageProviderMock.Object);

            ITestDataGenerator<CreateUserCommand> testDataGenerator = new CreateUserCommandDataGenerator();
            _command = testDataGenerator.Create();
        }

        [Fact]
        public async Task Validate_ValidCommand_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            _command.FirstName = "John";
            _command.LastName = "Doe";
            _command.EmailAddress = "john.doe@example.com";
            _command.TelephoneNumber = "1234567890";
            _command.RoleName = "User";

            // Act
            var result = await _validator.TestValidateAsync(_command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_EmailAddressAlreadyExists_ShouldHaveValidationError()
        {
            // Arrange
            _unitOfWorkMock.Setup(uw => uw.UserRepository.ExistsAsync(
                It.IsAny<Expression<Func<Domain.CVS.Domain.User, bool>>>(),
                    It.IsAny<CancellationToken>()
                )).ReturnsAsync(true);

            _command.EmailAddress = "john.doe@example.com";

            // Act
            var result = await _validator.TestValidateAsync(_command);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.EmailAddress);
        }

        [Fact]
        public async Task Validate_TelephoneNumberAlreadyExists_ShouldHaveValidationError()
        {
            // Arrange
            _unitOfWorkMock.Setup(uw => uw.UserRepository.ExistsAsync(
                It.IsAny<Expression<Func<Domain.CVS.Domain.User, bool>>>(),
                    It.IsAny<CancellationToken>()
                )).ReturnsAsync(true);

            _command.TelephoneNumber = "1234567890";

            // Act
            var result = await _validator.TestValidateAsync(_command);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.TelephoneNumber);
        }

        [Fact]
        public async Task Validate_FirstNameIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { FirstName = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.FirstName);
        }

        [Fact]
        public async Task Validate_FirstNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { FirstName = string.Empty };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.FirstName);
        }

        [Fact]
        public async Task Validate_FirstNameIsTooLong_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                FirstName = FakerConfiguration.Faker.Random.String2(Domain.CVS.Constants.UserConstants.FirstNameMaxLength + 1)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.FirstName);
        }

        [Fact]
        public async Task Validate_LastNameIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { LastName = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.LastName);
        }

        [Fact]
        public async Task Validate_LastNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { LastName = string.Empty };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.LastName);
        }

        [Fact]
        public async Task Validate_LastNameIsTooLong_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                LastName = FakerConfiguration.Faker.Random.String2(Domain.CVS.Constants.UserConstants.LastNameMaxLength + 1)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.LastName);
        }

        [Fact]
        public async Task Validate_RoleNameIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { RoleName = null };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.RoleName);
        }

        [Fact]
        public async Task Validate_EmailAddressIsInvalid_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { EmailAddress = "invalid-email" };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.EmailAddress);
        }

        [Fact]
        public async Task Validate_TelephoneNumberIsTooLong_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with
            {
                TelephoneNumber = FakerConfiguration.Faker.Random.String2(Domain.CVS.Constants.UserConstants.TelephoneNumberMaxLength + 1)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.TelephoneNumber);
        }

        [Fact]
        public async Task Validate_RoleNameIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = _command with { RoleName = string.Empty };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.RoleName);
        }
    }
}
