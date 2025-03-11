using Application.Authentication.Commands.Login;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Infrastructure.Identity;
using Moq;

namespace Application.UnitTests.Authentication.Commands.Login
{
    public class LoginCommandTests
    {
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;

        public LoginCommandTests()
        {
            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();
            _resourceMessageProviderMock
                .Setup(m => m.GetMessage(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns((Type type, string key, object[] args) =>
                    $"Mock validation message for Type: {type.Name}, Key: {key}, Args: {string.Join(", ", args ?? [])}");
        }

        [Fact]
        public async Task Handle_LoginUser_ShouldReturnTrue()
        {
            // Arrange
            var identityServiceMock = new Mock<IIdentityService>();
            var isUserLoggedIn = true;
            var user = new AuthenticationUser()
            {
                Salt = []
            };
            var loggedInUser = new LoggedInResult(isUserLoggedIn, user, []);

            identityServiceMock.Setup(mis => mis.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(loggedInUser);

            var bearerToken = "test";
            var bearerTokenServiceMock = new Mock<IBearerTokenService>();
            bearerTokenServiceMock.Setup(btsm => btsm.GenerateBearerTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<IList<string>>())).ReturnsAsync(bearerToken);

            var refreshToken = "test";
            var refreshTokenServiceMock = new Mock<ITokenService>();
            refreshTokenServiceMock.Setup(rtsm => rtsm.GenerateTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<string>())).ReturnsAsync(refreshToken);

            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock.Setup(mock => mock.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var handler = new LoginCommandHandler(identityServiceMock.Object, bearerTokenServiceMock.Object, refreshTokenServiceMock.Object, _resourceMessageProviderMock.Object, emailServiceMock.Object);

            var command = new LoginCommand
            {
                UserName = "Test",
                Password = "Test1234"
            };

            var loginCommandDto = new LoginCommandDto
            {
                Success = isUserLoggedIn,
                BearerToken = bearerToken,
                RefreshToken = refreshToken
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Value.Should().BeEquivalentTo(loginCommandDto);
        }

        [Fact]
        public async Task Handle_LoginUser_ShouldThrowInvalidLoginException()
        {
            // Arrange
            var isUserLoggedIn = false;
            var loggedInUser = new LoggedInResult(isUserLoggedIn);

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mis => mis.LoginAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(loggedInUser);

            var bearerTokenServiceMock = new Mock<IBearerTokenService>();

            var refreshToken = "test";
            var refreshTokenServiceMock = new Mock<ITokenService>();
            refreshTokenServiceMock.Setup(rtsm => rtsm.GenerateTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<string>())).ReturnsAsync(refreshToken);

            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock.Setup(mock => mock.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            var handler = new LoginCommandHandler(identityServiceMock.Object, bearerTokenServiceMock.Object, refreshTokenServiceMock.Object, _resourceMessageProviderMock.Object, emailServiceMock.Object);

            var command = new LoginCommand
            {
                UserName = "Test",
                Password = "Test1234"
            };

            var loginCommandDto = new LoginCommandDto
            {
                Success = isUserLoggedIn
            };

            // Act & Assert
            Func<Task> act = async () => await handler.Handle(command, default);

            // Assert
            await act.Should().ThrowAsync<InvalidLoginException>();
        }
    }
}
