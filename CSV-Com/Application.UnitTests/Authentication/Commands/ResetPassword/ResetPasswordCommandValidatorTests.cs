using Application.Authentication.Commands.ResetPassword;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Moq;
using TestData;

namespace Application.UnitTests.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandValidatorTests
    {
        [Fact]
        public async Task Handle_SuccessFlow_ResultSuccessIsTrue()
        {
            // Arrange
            var password = FakerConfiguration.Faker.Internet.Password();

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Result.Success);

            var handler = new ResetPasswordCommandHandler(identityServiceMock.Object);

            var command = new ResetPasswordCommand
            {
                EmailAddress = FakerConfiguration.Faker.Person.Email,
                Token = "Token",
                NewPassword = password,
                NewPasswordRepeat = password
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_ResetPasswordAsyncFails_ResultContainsErrorMessage()
        {
            // Arrange
            var password = FakerConfiguration.Faker.Internet.Password();
            var message = "This is an error!";
            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Result.Failure([message]));

            var handler = new ResetPasswordCommandHandler(identityServiceMock.Object);

            var command = new ResetPasswordCommand
            {
                EmailAddress = FakerConfiguration.Faker.Person.Email,
                Token = "Token",
                NewPassword = password,
                NewPasswordRepeat = password
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain(message);
        }

        [Fact]
        public async Task Handle_ResetPasswordAsyncThrowsException_ResultSuccessIsFalse()
        {
            // Arrange
            var password = FakerConfiguration.Faker.Internet.Password();
            var exception = new Exception();

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(exception);

            var handler = new ResetPasswordCommandHandler(identityServiceMock.Object);

            var command = new ResetPasswordCommand
            {
                EmailAddress = FakerConfiguration.Faker.Person.Email,
                Token = "Token",
                NewPassword = password,
                NewPasswordRepeat = password
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ResetPasswordAsyncThrowsException_ErrorContainsExceptionMessage()
        {
            // Arrange
            var password = FakerConfiguration.Faker.Internet.Password();
            var message = "This is an error!";
            var exception = new Exception(message);

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(exception);

            var handler = new ResetPasswordCommandHandler(identityServiceMock.Object);

            var command = new ResetPasswordCommand
            {
                EmailAddress = FakerConfiguration.Faker.Person.Email,
                Token = "Token",
                NewPassword = password,
                NewPasswordRepeat = password
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Errors.Should().Contain(message);
        }
    }
}
