using Application.Authentication.Commands.RequestResetPassword;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Moq;
using TestData;

namespace Application.UnitTests.Authentication.Commands.RequestResetPassword
{
    public class RequestResetPasswordCommandTests
    {
        [Fact]
        public async Task Handle_SuccessFlow_ResultSuccessIsTrue()
        {
            // Arrange
            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.GetResetPasswordEmailToken(It.IsAny<string>())).ReturnsAsync("Token");

            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock.Setup(mock => mock.SendEmailAsync(It.IsAny<EmailMessageDto>(), It.IsAny<string>(), It.IsAny<object>()));

            var handler = new RequestResetPasswordCommandHandler(identityServiceMock.Object, emailServiceMock.Object);

            var command = new RequestResetPasswordCommand
            {
                EmailAddress = FakerConfiguration.Faker.Person.Email
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_SendResetPasswordEmailThrowsException_SuccessIsFalse()
        {
            // Arrange
            var error = "This is an error!";
            var exception = new Exception(error);

            var identityServiceMock = new Mock<IIdentityService>();
            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock.Setup(mock => mock.SendEmailAsync(It.IsAny<EmailMessageDto>(), It.IsAny<string>(), It.IsAny<object>())).ThrowsAsync(exception);

            var handler = new RequestResetPasswordCommandHandler(identityServiceMock.Object, emailServiceMock.Object);

            var command = new RequestResetPasswordCommand
            {
                EmailAddress = FakerConfiguration.Faker.Person.Email
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_GetResetPasswordEmailToken_ErrorContainsExceptionMessage()
        {
            // Arrange
            var error = "This is an error!";
            var exception = new Exception(error);

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.GetResetPasswordEmailToken(It.IsAny<string>())).ThrowsAsync(exception);

            var emailServiceMock = new Mock<IEmailService>();

            var handler = new RequestResetPasswordCommandHandler(identityServiceMock.Object, emailServiceMock.Object);

            var command = new RequestResetPasswordCommand
            {
                EmailAddress = FakerConfiguration.Faker.Person.Email
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Errors.Should().Contain(error);
        }
    }
}
