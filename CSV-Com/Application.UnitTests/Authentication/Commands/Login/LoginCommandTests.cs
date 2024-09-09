using Application.Authentication.Commands.Login;
using Application.Common.Interfaces.Authentication;
using Moq;

namespace Application.UnitTests.Authentication.Commands.Login
{
    public class LoginCommandTests
    {
        [Fact]
        public async Task Handle_LoginUser_ShouldReturnTrue()
        {
            // Arrange
            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mis => mis.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var handler = new LoginCommandHandler(identityServiceMock.Object);

            var command = new LoginCommand
            {
                UserName = "Test",
                Password = "Test1234"
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_LoginUser_ShouldReturnFalse()
        {
            // Arrange
            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mis => mis.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var handler = new LoginCommandHandler(identityServiceMock.Object);

            var command = new LoginCommand
            {
                UserName = "Test",
                Password = "Test1234"
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().BeFalse();
        }
    }
}
