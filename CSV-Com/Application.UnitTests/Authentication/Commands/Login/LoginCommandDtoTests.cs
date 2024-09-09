using Application.Authentication.Commands.Login;
using Application.Common.Interfaces.Authentication;
using Moq;

namespace Application.UnitTests.Authentication.Commands.Login
{
    public class LoginCommandDtoTests
    {

        [Fact]
        public async Task Handle_Login_LoginResultShouldBeTrue()
        {
            // Arrange
            var isUserLoggedIn = true;

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mis => mis.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(isUserLoggedIn);

            var handler = new LoginCommandHandler(identityServiceMock.Object);

            var command = new LoginCommand
            {
                UserName = "Test",
                Password = "Test1234"
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().Be(isUserLoggedIn);
        }

        [Fact]
        public async Task Handle_Login_LoginResultShouldBeFalse()
        {
            // Arrange
            var isUserLoggedIn = false;

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mis => mis.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(isUserLoggedIn);

            var handler = new LoginCommandHandler(identityServiceMock.Object);

            var command = new LoginCommand
            {
                UserName = "Test",
                Password = "Test1234"
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().Be(isUserLoggedIn);
        }
    }
}
