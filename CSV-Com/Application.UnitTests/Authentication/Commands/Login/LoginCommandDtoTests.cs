using Application.Authentication.Commands.Login;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Domain.Authentication.Domain;
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
            var user = new AuthenticationUser()
            {
                Salt = []
            };
            var loggedInUser = new LoggedInResult(isUserLoggedIn, user, new List<string>());

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mis => mis.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(loggedInUser);

            var bearerToken = "test";
            var bearerTokenServiceMock = new Mock<IBearerTokenService>();
            bearerTokenServiceMock.Setup(btsm => btsm.GenerateBearerTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<IList<string>>())).ReturnsAsync(bearerToken);

            var handler = new LoginCommandHandler(identityServiceMock.Object, bearerTokenServiceMock.Object);

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
            var loggedInUser = new LoggedInResult(isUserLoggedIn);

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mis => mis.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(loggedInUser);

            var bearerTokenServiceMock = new Mock<IBearerTokenService>();

            var handler = new LoginCommandHandler(identityServiceMock.Object, bearerTokenServiceMock.Object);

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
        public async Task Handle_Login_BearerTokenShouldNotBeEmpty()
        {
            // Arrange
            var isUserLoggedIn = true;
            var user = new AuthenticationUser()
            {
                Salt = []
            };
            var loggedInUser = new LoggedInResult(isUserLoggedIn, user, new List<string>());

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mis => mis.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(loggedInUser);

            var bearerToken = "test";
            var bearerTokenServiceMock = new Mock<IBearerTokenService>();
            bearerTokenServiceMock.Setup(btsm => btsm.GenerateBearerTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<IList<string>>())).ReturnsAsync(bearerToken);

            var handler = new LoginCommandHandler(identityServiceMock.Object, bearerTokenServiceMock.Object);

            var command = new LoginCommand
            {
                UserName = "Test",
                Password = "Test1234"
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.BearerToken.Should().NotBeEmpty().And.Be(bearerToken);
        }

        [Fact]
        public async Task Handle_Login_BearerTokenShouldBeNull()
        {
            // Arrange
            var isUserLoggedIn = false;
            var loggedInUser = new LoggedInResult(isUserLoggedIn);

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mis => mis.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(loggedInUser);

            var bearerTokenServiceMock = new Mock<IBearerTokenService>();
            bearerTokenServiceMock.Setup(btsm => btsm.GenerateBearerTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<IList<string>>())).ReturnsAsync(string.Empty);

            var handler = new LoginCommandHandler(identityServiceMock.Object, bearerTokenServiceMock.Object);

            var command = new LoginCommand
            {
                UserName = "Test",
                Password = "Test1234"
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.BearerToken.Should().BeNull();
        }
    }
}
