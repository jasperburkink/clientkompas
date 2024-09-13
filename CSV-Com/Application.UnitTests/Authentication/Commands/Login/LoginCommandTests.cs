using Application.Authentication.Commands.Login;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Domain.Authentication.Domain;
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
            var isUserLoggedIn = true;
            var user = new AuthenticationUser()
            {
                Salt = new byte[0]
            };
            var loggedInUser = new LoggedInResult(isUserLoggedIn, user, new List<string>());

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

            var loginCommandDto = new LoginCommandDto
            {
                Success = isUserLoggedIn,
                BearerToken = bearerToken
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeEquivalentTo(loginCommandDto);
        }

        [Fact]
        public async Task Handle_LoginUser_ShouldReturnFalse()
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

            var loginCommandDto = new LoginCommandDto
            {
                Success = isUserLoggedIn
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeEquivalentTo(loginCommandDto);
        }
    }
}
