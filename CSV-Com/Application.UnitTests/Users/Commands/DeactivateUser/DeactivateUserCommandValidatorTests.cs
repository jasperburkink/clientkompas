using System.Linq.Expressions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Users.Commands.DeactivateUser;
using FluentValidation.TestHelper;
using Moq;

namespace Application.UnitTests.Users.Commands.DeactivateUser
{
    public class DeactivateUserCommandValidatorTests
    {
        private readonly DeactivateUserCommandValidator _validator;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;

        public DeactivateUserCommandValidatorTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();
            _resourceMessageProviderMock
                .Setup(m => m.GetMessage(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns((Type type, string key, object[] args) =>
                    $"Mock validation message for Type: {type.Name}, Key: {key}, Args: {string.Join(", ", args ?? [])}");

            _validator = new DeactivateUserCommandValidator(_unitOfWorkMock.Object, _resourceMessageProviderMock.Object);
        }


        [Fact]
        public async Task Validate_ValidCommand_ShouldNotHaveAnyValidationErrors()
        {
            // Arrange
            var command = new DeactivateUserCommand()
            {
                Id = 5
            };

            _unitOfWorkMock.Setup(uw => uw.UserRepository.ExistsAsync(
                It.IsAny<Expression<Func<Domain.CVS.Domain.User, bool>>>(),
                    It.IsAny<CancellationToken>()
                )).ReturnsAsync(true);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Validate_UserDoesNotExists_ShouldHaveValidationError()
        {
            // Arrange
            var command = new DeactivateUserCommand()
            {
                Id = 5
            };

            _unitOfWorkMock.Setup(uw => uw.UserRepository.ExistsAsync(
                It.IsAny<Expression<Func<Domain.CVS.Domain.User, bool>>>(),
                    It.IsAny<CancellationToken>()
                )).ReturnsAsync(false);

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(u => u.Id);
        }
    }
}
