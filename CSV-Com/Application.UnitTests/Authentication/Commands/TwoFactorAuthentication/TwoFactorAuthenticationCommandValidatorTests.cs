using Application.Authentication.Commands.TwoFactorAuthentication;
using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using FluentValidation.TestHelper;
using Moq;

namespace Application.UnitTests.Authentication.Commands.TwoFactorAuthentication
{
    public class TwoFactorAuthenticationCommandValidatorTests
    {
        private readonly TwoFactorAuthenticationCommandValidator _validator;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;

        public TwoFactorAuthenticationCommandValidatorTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();
            _resourceMessageProviderMock
                .Setup(m => m.GetMessage(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns((Type type, string key, object[] args) =>
                    $"Mock validation message for Type: {type.Name}, Key: {key}, Args: {string.Join(", ", args ?? [])}");

            _validator = new TwoFactorAuthenticationCommandValidator(_unitOfWorkMock.Object, _resourceMessageProviderMock.Object);
        }

        [Fact]
        public async Task Validate_ValidCommand_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new TwoFactorAuthenticationCommand
            {
                UserId = Guid.NewGuid().ToString(),
                Token = "123456"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_UserIdIsNull_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var command = new TwoFactorAuthenticationCommand
            {
                UserId = null,
                Token = "123456"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(dto => dto.UserId);
        }

        [Fact]
        public async Task Validate_UserIdIsEmpty_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var command = new TwoFactorAuthenticationCommand
            {
                UserId = string.Empty,
                Token = "123456"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(dto => dto.UserId);
        }

        [Fact]
        public async Task Validate_TokenIsNull_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var command = new TwoFactorAuthenticationCommand
            {
                UserId = Guid.NewGuid().ToString(),
                Token = null
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(dto => dto.Token);
        }

        [Fact]
        public async Task Validate_TokenIsEmpty_ShouldNotHaveValidationErrors()
        {
            // Arrange
            var command = new TwoFactorAuthenticationCommand
            {
                UserId = Guid.NewGuid().ToString(),
                Token = string.Empty
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(dto => dto.Token);
        }
    }
}
