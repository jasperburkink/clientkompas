using Application.Authentication.Commands.ResendTwoFactorAuthenticationToken;
using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using FluentValidation.TestHelper;
using Infrastructure.Identity;
using Moq;

namespace Application.UnitTests.Authentication.Commands.ResendTwoFactorAuthenticationToken
{
    public class ResendTwoFactorAuthenticationTokenCommandValidatorTests
    {
        private readonly ResendTwoFactorAuthenticationTokenCommandValidator _validator;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;

        public ResendTwoFactorAuthenticationTokenCommandValidatorTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();
            _resourceMessageProviderMock
                .Setup(m => m.GetMessage(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns((Type type, string key, object[] args) =>
                    $"Mock validation message for Type: {type.Name}, Key: {key}, Args: {string.Join(", ", args ?? [])}");

            _validator = new ResendTwoFactorAuthenticationTokenCommandValidator(_unitOfWorkMock.Object, _resourceMessageProviderMock.Object);
        }

        [Fact]
        public async Task Handle_SuccessFlow_NoValidationErrors()
        {
            // Arrange
            var command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = Guid.NewGuid().ToString(),
                TwoFactorPendingToken = nameof(TwoFactorPendingToken)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Handle_UserIdIsNull_ShouldHaveValidationErrors()
        {
            // Arrange
            var command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = null,
                TwoFactorPendingToken = nameof(TwoFactorPendingToken)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.UserId);
        }

        [Fact]
        public async Task Handle_UserIdIsEmpty_ShouldHaveValidationErrors()
        {
            // Arrange
            var command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = string.Empty,
                TwoFactorPendingToken = nameof(TwoFactorPendingToken)
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.UserId);
        }

        [Fact]
        public async Task Handle_TwoFactorPendingTokenNull_ShouldHaveValidationErrors()
        {
            // Arrange
            var command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = Guid.NewGuid().ToString(),
                TwoFactorPendingToken = null
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.TwoFactorPendingToken);
        }

        [Fact]
        public async Task Handle_TwoFactorPendingTokenIsEmpty_ShouldHaveValidationErrors()
        {
            // Arrange
            var command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = Guid.NewGuid().ToString(),
                TwoFactorPendingToken = string.Empty
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.TwoFactorPendingToken);
        }
    }
}
