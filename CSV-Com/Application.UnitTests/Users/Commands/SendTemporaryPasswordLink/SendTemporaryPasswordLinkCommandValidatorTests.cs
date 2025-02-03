using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;
using Application.Users.Commands.SendTemporaryPasswordLinkCommand;
using FluentValidation.TestHelper;
using Moq;

namespace Application.UnitTests.Users.Commands.SendTemporaryPasswordLink
{
    public class SendTemporaryPasswordLinkCommandValidatorTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;
        private readonly SendTemporaryPasswordLinkCommandValidator _validator;
        private readonly SendTemporaryPasswordLinkCommand _command;

        public SendTemporaryPasswordLinkCommandValidatorTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();

            _resourceMessageProviderMock
                .Setup(rmp => rmp.GetMessage(typeof(AuthenticationValidationRules), "UserIdRequired"))
                .Returns("User ID is required.");

            _validator = new SendTemporaryPasswordLinkCommandValidator(_unitOfWorkMock.Object, _resourceMessageProviderMock.Object);

            _command = new SendTemporaryPasswordLinkCommand { UserId = "validUserId" };
        }

        [Fact]
        public async Task Validate_UserIdIsValid_ShouldNotHaveAnyValidationErrors()
        {
            // Act
            var result = await _validator.TestValidateAsync(_command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_UserIdIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            _command.UserId = string.Empty;

            // Act
            var result = await _validator.TestValidateAsync(_command);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.UserId)
                .WithErrorMessage("User ID is required.");
        }

        [Fact]
        public async Task Validate_UserIdIsNull_ShouldHaveValidationError()
        {
            // Arrange
            _command.UserId = null;

            // Act
            var result = await _validator.TestValidateAsync(_command);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.UserId)
                .WithErrorMessage("User ID is required.");
        }
    }
}
