using Application.Authentication.Commands.Logout;
using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using FluentValidation.TestHelper;
using Moq;

namespace Application.UnitTests.Authentication.Commands.Logout
{
    public class LogoutCommandValidatorTests
    {
        private readonly LogoutCommandValidator _validator;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;

        public LogoutCommandValidatorTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();
            _resourceMessageProviderMock
                .Setup(m => m.GetMessage(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns((Type type, string key, object[] args) =>
                    $"Mock validation message for Type: {type.Name}, Key: {key}, Args: {string.Join(", ", args ?? [])}");

            _validator = new LogoutCommandValidator(_unitOfWorkMock.Object, _resourceMessageProviderMock.Object);
        }

        [Fact]
        public async Task Validate_SuccessFlow_NoValidationErrors()
        {
            // Arrange
            var refreshToken = "RefreshToken";

            var command = new LogoutCommand
            {
                RefreshToken = refreshToken
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_RefreshTokenIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LogoutCommand
            {
                RefreshToken = null
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(f => f.RefreshToken);
        }

        [Fact]
        public async Task Validate_RefreshTokenIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            var command = new LogoutCommand
            {
                RefreshToken = string.Empty
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(f => f.RefreshToken);
        }
    }
}
