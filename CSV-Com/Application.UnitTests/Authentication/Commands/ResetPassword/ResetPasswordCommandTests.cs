using Application.Authentication.Commands.ResetPassword;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Moq;
using TestData;

namespace Application.UnitTests.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandTests
    {
        [Fact]
        public async Task Handle_CorrectFlow_SuccessIsTrue()
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
        public async Task Handle_EmailAddressIsNull_SuccessIsFalse()
        {
            // Arrange
            var password = FakerConfiguration.Faker.Internet.Password();

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Result.Success);

            var handler = new ResetPasswordCommandHandler(identityServiceMock.Object);

            var command = new ResetPasswordCommand
            {
                EmailAddress = null,
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
        public async Task Handle_TokenIsNull_SuccessIsFalse()
        {
            // Arrange
            var password = FakerConfiguration.Faker.Internet.Password();

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Result.Success);

            var handler = new ResetPasswordCommandHandler(identityServiceMock.Object);

            var command = new ResetPasswordCommand
            {
                EmailAddress = FakerConfiguration.Faker.Person.Email,
                Token = null,
                NewPassword = password,
                NewPasswordRepeat = password
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_NewPasswordIsNull_SuccessIsFalse()
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
                NewPassword = null,
                NewPasswordRepeat = password
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_ResetPasswordAsyncThrowsException_SuccessIsFalse()
        {
            // Arrange
            var password = FakerConfiguration.Faker.Internet.Password();

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception("Test"));

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
        public async Task Handle_ResetPasswordAsyncThrowsException_ErrorIsReturned()
        {
            // Arrange
            var password = FakerConfiguration.Faker.Internet.Password();

            var identityServiceMock = new Mock<IIdentityService>();
            var errorMessage = "Test";
            identityServiceMock.Setup(mock => mock.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception(errorMessage));

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
            result.Errors.Should().Contain(errorMessage);
        }

        [Fact]
        public async Task Handle_ResetPasswordAsyncReturnFailure_SuccessIsFalse()
        {
            // Arrange
            var password = FakerConfiguration.Faker.Internet.Password();

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Result.Failure(["Test"]));

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
        public async Task Handle_ResetPasswordAsyncReturnFailure_ErrorIsReturned()
        {
            // Arrange
            var password = FakerConfiguration.Faker.Internet.Password();
            var errorMessage = "Test!";

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Result.Failure([errorMessage]));

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
            result.Errors.Contains(errorMessage);
        }
    }
}
