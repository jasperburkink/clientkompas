using Application.Authentication.Commands.Logout;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Domain;
using Moq;

namespace Application.UnitTests.Authentication.Commands.Logout
{
    public class LogoutCommandTests
    {
        [Fact]
        public async Task Handle_SuccessFlow_LoggedOut()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.LogoutAsync());

            var refreshToken = new Infrastructure.Identity.RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                LoginProvider = "Test",
                Name = "Test1",
                UserId = userId.ToString(),
                Value = "Test1"
            };

            var refreshTokenUser1 = new Infrastructure.Identity.RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                LoginProvider = "Test",
                Name = "Test2",
                UserId = userId.ToString(),
                Value = "Test2"
            };

            var refreshTokenUser2 = new Infrastructure.Identity.RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                LoginProvider = "Test",
                Name = "Test3",
                UserId = userId.ToString(),
                Value = "Test3"
            };

            var userRefreshTokens = new List<IToken>
            {
                refreshToken,
                refreshTokenUser1,
                refreshTokenUser2
            };

            var refreshTokenServiceMock = new Mock<ITokenService>();
            refreshTokenServiceMock.Setup(mock => mock.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(refreshToken);
            refreshTokenServiceMock.Setup(mock => mock.GetValidTokensByUserAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(userRefreshTokens);

            var command = new LogoutCommand()
            {
                RefreshToken = refreshToken.Value
            };

            var handler = new LogoutCommandHandler(identityServiceMock.Object, refreshTokenServiceMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            identityServiceMock.Verify(mock => mock.LogoutAsync(), Times.Once);
            refreshTokenServiceMock.Verify(mock => mock.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            refreshTokenServiceMock.Verify(mock => mock.GetValidTokensByUserAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            refreshTokenServiceMock.Verify(mock => mock.RevokeTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(userRefreshTokens.Count));
        }

        [Fact]
        public async Task Handle_NoValidTokens_NoTokensRevoked()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.LogoutAsync());

            var refreshToken = new Infrastructure.Identity.RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = true,
                LoginProvider = "Test",
                Name = "Test1",
                UserId = userId.ToString(),
                Value = "Test1"
            };

            var userRefreshTokens = new List<IToken>
            {
            };

            var refreshTokenServiceMock = new Mock<ITokenService>();
            refreshTokenServiceMock.Setup(mock => mock.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(refreshToken);
            refreshTokenServiceMock.Setup(mock => mock.GetValidTokensByUserAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(userRefreshTokens);

            var command = new LogoutCommand()
            {
                RefreshToken = refreshToken.Value
            };

            var handler = new LogoutCommandHandler(identityServiceMock.Object, refreshTokenServiceMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            identityServiceMock.Verify(mock => mock.LogoutAsync(), Times.Once);
            refreshTokenServiceMock.Verify(mock => mock.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            refreshTokenServiceMock.Verify(mock => mock.GetValidTokensByUserAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            refreshTokenServiceMock.Verify(mock => mock.RevokeTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Handle_RefreshTokenDoesNotExists_OnlyLogOutIsCalled()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.LogoutAsync());

            var refreshTokenServiceMock = new Mock<ITokenService>();
            refreshTokenServiceMock.Setup(mock => mock.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((IToken)null);

            var command = new LogoutCommand()
            {
                RefreshToken = "Test1"
            };

            var handler = new LogoutCommandHandler(identityServiceMock.Object, refreshTokenServiceMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            identityServiceMock.Verify(mock => mock.LogoutAsync(), Times.Once);
            refreshTokenServiceMock.Verify(mock => mock.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            refreshTokenServiceMock.Verify(mock => mock.GetValidTokensByUserAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            refreshTokenServiceMock.Verify(mock => mock.RevokeTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }
}
