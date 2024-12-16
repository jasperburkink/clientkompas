using Application.Authentication.Commands.RequestResetPassword;
using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using FluentValidation.TestHelper;
using Moq;
using TestData;

namespace Application.UnitTests.Authentication.Commands.RequestResetPassword
{
    public class RequestResetPasswordCommandValidatorTests
    {
        private readonly RequestResetPasswordCommandValidator _validator;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;

        public RequestResetPasswordCommandValidatorTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();
            _resourceMessageProviderMock
                .Setup(m => m.GetMessage(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns((Type type, string key, object[] args) =>
                    $"Mock validation message for Type: {type.Name}, Key: {key}, Args: {string.Join(", ", args ?? [])}");

            _validator = new RequestResetPasswordCommandValidator(_unitOfWorkMock.Object, _resourceMessageProviderMock.Object);
        }

        [Fact]
        public async Task Handle_SuccessFlow_NoValidationErrors()
        {
            // Arrange
            var command = new RequestResetPasswordCommand
            {
                EmailAddress = FakerConfiguration.Faker.Person.Email
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public async Task Handle_InvalidEmailAddres_ShouldHaveValidationErrors()
        {
            // Arrange
            var command = new RequestResetPasswordCommand
            {
                EmailAddress = "Not a valid emailaddress"
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.EmailAddress);
        }

        [Fact]
        public async Task Handle_EmailAddresIsNull_ShouldHaveValidationErrors()
        {
            // Arrange
            var command = new RequestResetPasswordCommand
            {
                EmailAddress = null
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.EmailAddress);
        }

        [Fact]
        public async Task Handle_EmailAddresIsEmpty_ShouldHaveValidationErrors()
        {
            // Arrange
            var command = new RequestResetPasswordCommand
            {
                EmailAddress = string.Empty
            };

            // Act
            var result = await _validator.TestValidateAsync(command);

            // Assert
            result.ShouldHaveValidationErrorFor(cmd => cmd.EmailAddress);
        }
    }
}
