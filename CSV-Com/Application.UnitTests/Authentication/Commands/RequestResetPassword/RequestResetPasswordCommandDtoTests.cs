using Application.Authentication.Commands.RequestResetPassword;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Moq;
using TestData;

namespace Application.UnitTests.Authentication.Commands.RequestResetPassword
{
    public class RequestResetPasswordCommandDtoTests
    {
        [Fact]
        public async Task Handle_EmailAddressIsSet_ResultSuccessIsTrue()
        {
            // Arrange
            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.SendResetPasswordEmailAsync(It.IsAny<string>())).ReturnsAsync(Result.Success);

            var handler = new RequestResetPasswordCommandHandler(identityServiceMock.Object);

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
        public async Task Handle_EmailAddressIsNull_SuccessIsFalse()
        {
            // Arrange
            var identityServiceMock = new Mock<IIdentityService>();
            var handler = new RequestResetPasswordCommandHandler(identityServiceMock.Object);

            var command = new RequestResetPasswordCommand
            {
                EmailAddress = null
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().BeFalse();
        }

        [Fact]
        public async Task Handle_EmailAddressIsEmpty_SuccessIsFalse()
        {
            // Arrange
            var identityServiceMock = new Mock<IIdentityService>();
            var handler = new RequestResetPasswordCommandHandler(identityServiceMock.Object);

            var command = new RequestResetPasswordCommand
            {
                EmailAddress = string.Empty
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().BeFalse();
        }
    }
}
