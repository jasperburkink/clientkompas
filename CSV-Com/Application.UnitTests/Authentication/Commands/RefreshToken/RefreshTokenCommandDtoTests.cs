using System.Text;
using Application.Authentication.Commands.RefreshToken;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Domain;
using Infrastructure.Identity;
using Moq;

namespace Application.UnitTests.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandDtoTests
    {
        [Fact]
        public async Task Handle_GetRefreshToken_SuccessShouldBeTrue()
        {
            // Arrange
            var refreshToken = "RefreshToken";
            var bearerToken = "BearerToken";
            var refreshTokenNew = "RefreshTokenNew";

            var command = new RefreshTokenCommand
            {
                RefreshToken = refreshToken
            };

            var refreshTokenMock = new Mock<IAuthenticationToken>();

            var user = new AuthenticationUser
            {
                Id = Guid.NewGuid().ToString(),
                Salt = Encoding.UTF8.GetBytes("Salt")
            };

            var refreshTokenServiceMock = new Mock<ITokenService>();
            refreshTokenServiceMock.Setup(mock => mock.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(refreshTokenMock.Object);
            refreshTokenServiceMock.Setup(mock => mock.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            refreshTokenServiceMock.Setup(mock => mock.RevokeTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            refreshTokenServiceMock.Setup(mock => mock.GenerateTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<string>())).ReturnsAsync(refreshTokenNew);

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
        }

        [Fact]
        public async Task Handle_GetRefreshToken_ReturnsNewRefreshToken()
        {
            // Arrange
            var refreshToken = "RefreshToken";
            var bearerToken = "BearerToken";
            var refreshTokenNew = "RefreshTokenNew";

            var command = new RefreshTokenCommand
            {
                RefreshToken = refreshToken
            };

            var refreshTokenMock = new Mock<IAuthenticationToken>();

            var user = new AuthenticationUser
            {
                Id = Guid.NewGuid().ToString(),
                Salt = Encoding.UTF8.GetBytes("Salt")
            };

            var refreshTokenServiceMock = new Mock<ITokenService>();
            refreshTokenServiceMock.Setup(mock => mock.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(refreshTokenMock.Object);
            refreshTokenServiceMock.Setup(mock => mock.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            refreshTokenServiceMock.Setup(mock => mock.RevokeTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            refreshTokenServiceMock.Setup(mock => mock.GenerateTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<string>())).ReturnsAsync(refreshTokenNew);

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
            result.RefreshToken.Should().Be(refreshTokenNew);
        }

        [Fact]
        public async Task Handle_GetRefreshToken_ReturnsNewBearerToken()
        {
            // Arrange
            var refreshToken = "RefreshToken";
            var bearerToken = "BearerToken";
            var refreshTokenNew = "RefreshTokenNew";

            var command = new RefreshTokenCommand
            {
                RefreshToken = refreshToken
            };

            var refreshTokenMock = new Mock<IAuthenticationToken>();

            var user = new AuthenticationUser
            {
                Id = Guid.NewGuid().ToString(),
                Salt = Encoding.UTF8.GetBytes("Salt")
            };

            var refreshTokenServiceMock = new Mock<ITokenService>();
            refreshTokenServiceMock.Setup(mock => mock.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(refreshTokenMock.Object);
            refreshTokenServiceMock.Setup(mock => mock.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            refreshTokenServiceMock.Setup(mock => mock.RevokeTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
            refreshTokenServiceMock.Setup(mock => mock.GenerateTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<string>())).ReturnsAsync(refreshTokenNew);

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
            result.BearerToken.Should().Be(bearerToken);
        }
    }
}
