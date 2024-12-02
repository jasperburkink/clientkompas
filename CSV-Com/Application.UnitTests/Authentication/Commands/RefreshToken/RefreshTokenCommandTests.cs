using System.Text;
using Application.Authentication.Commands.RefreshToken;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Domain;
using Moq;

namespace Application.UnitTests.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandTests
    {
        [Fact]
        public async Task Handle_SuccessFlow_ReturnsNewTokens()
        {
            // Arrange
            var refreshToken = "RefreshToken";
            var bearerToken = "BearerToken";
            var refreshTokenNew = "RefreshTokenNew";

            var command = new RefreshTokenCommand
            {
                RefreshToken = refreshToken
            };

            var refreshTokenMock = new Mock<IRefreshToken>();

            var user = new AuthenticationUser
            {
                Id = Guid.NewGuid().ToString(),
                Salt = Encoding.UTF8.GetBytes("Salt")
            };

            var refreshTokenServiceMock = new Mock<IRefreshTokenService>();
            refreshTokenServiceMock.Setup(mock => mock.GetRefreshTokenAsync(It.IsAny<string>())).ReturnsAsync(refreshTokenMock.Object);
            refreshTokenServiceMock.Setup(mock => mock.ValidateRefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            refreshTokenServiceMock.Setup(mock => mock.RevokeRefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>()));
            refreshTokenServiceMock.Setup(mock => mock.GenerateRefreshTokenAsync(It.IsAny<AuthenticationUser>())).ReturnsAsync(refreshTokenNew);

            var bearerTokenServiceMock = new Mock<IBearerTokenService>();
            bearerTokenServiceMock.Setup(mock => mock.GenerateBearerTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<IList<string>>())).ReturnsAsync(bearerToken);

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);
            identityServiceMock.Setup(mock => mock.GetRolesAsync(It.IsAny<string>())).ReturnsAsync([]);

            // Act
            var handler = new RefreshTokenCommandHandler(refreshTokenServiceMock.Object, bearerTokenServiceMock.Object, identityServiceMock.Object);
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.RefreshToken.Should().Be(refreshTokenNew);
            result.BearerToken.Should().Be(bearerToken);
        }

        [Fact]
        public async Task Handle_RequestRefreshTokenIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var command = new RefreshTokenCommand
            {
                RefreshToken = null
            };

            var refreshTokenServiceMock = new Mock<IRefreshTokenService>();

            var bearerTokenServiceMock = new Mock<IBearerTokenService>();

            var identityServiceMock = new Mock<IIdentityService>();

            var handler = new RefreshTokenCommandHandler(refreshTokenServiceMock.Object, bearerTokenServiceMock.Object, identityServiceMock.Object);

            // Act & Asert
            await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_RefreshTokenNotFound_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var refreshToken = "RefreshToken";

            var command = new RefreshTokenCommand
            {
                RefreshToken = refreshToken
            };

            var refreshTokenServiceMock = new Mock<IRefreshTokenService>();
            refreshTokenServiceMock.Setup(mock => mock.GetRefreshTokenAsync(It.IsAny<string>()));

            var bearerTokenServiceMock = new Mock<IBearerTokenService>();

            var identityServiceMock = new Mock<IIdentityService>();

            var handler = new RefreshTokenCommandHandler(refreshTokenServiceMock.Object, bearerTokenServiceMock.Object, identityServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_InvalidRefreshToken_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var refreshToken = "RefreshToken";

            var command = new RefreshTokenCommand
            {
                RefreshToken = refreshToken
            };

            var refreshTokenMock = new Mock<IRefreshToken>();

            var refreshTokenServiceMock = new Mock<IRefreshTokenService>();
            refreshTokenServiceMock.Setup(mock => mock.GetRefreshTokenAsync(It.IsAny<string>())).ReturnsAsync(refreshTokenMock.Object);
            refreshTokenServiceMock.Setup(mock => mock.ValidateRefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var bearerTokenServiceMock = new Mock<IBearerTokenService>();

            var identityServiceMock = new Mock<IIdentityService>();

            var handler = new RefreshTokenCommandHandler(refreshTokenServiceMock.Object, bearerTokenServiceMock.Object, identityServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, default));
        }

        [Fact]
        public async Task Handle_UserDoesNotExists_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var refreshToken = "RefreshToken";

            var command = new RefreshTokenCommand
            {
                RefreshToken = refreshToken
            };

            var refreshTokenMock = new Mock<IRefreshToken>();

            var refreshTokenServiceMock = new Mock<IRefreshTokenService>();
            refreshTokenServiceMock.Setup(mock => mock.GetRefreshTokenAsync(It.IsAny<string>())).ReturnsAsync(refreshTokenMock.Object);
            refreshTokenServiceMock.Setup(mock => mock.ValidateRefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            var bearerTokenServiceMock = new Mock<IBearerTokenService>();

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.GetUserAsync(It.IsAny<string>()));

            var handler = new RefreshTokenCommandHandler(refreshTokenServiceMock.Object, bearerTokenServiceMock.Object, identityServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => handler.Handle(command, default));
        }
    }
}
