using Application.Authentication.Commands.ResetPassword;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Moq;
using TestData;

namespace Application.UnitTests.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandDtoTests
    {
        [Fact]
        public async Task Handle_CorrectFlow_ResultSuccessIsTrue()
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
        public async Task Handle_CorrectFlow_ResultErrorsIsEmpty()
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
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ResetPasswordReturnsFailure_ResultSuccessIsFalse()
        {
            // Arrange
            var password = FakerConfiguration.Faker.Internet.Password();
            var error = "This is an error.";

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Result.Failure([error]));

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
        public async Task Handle_ResetPasswordReturnsFailure_ResultContainsError()
        {
            // Arrange
            var password = FakerConfiguration.Faker.Internet.Password();
            var error = "This is an error.";

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Result.Failure([error]));

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
            result.Errors.Should().Contain(error);
        }
    }
}
