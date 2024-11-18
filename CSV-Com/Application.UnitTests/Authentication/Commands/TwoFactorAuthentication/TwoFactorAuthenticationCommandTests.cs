using Application.Authentication.Commands.TwoFactorAuthentication;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using Moq;

namespace Application.UnitTests.Authentication.Commands.TwoFactorAuthentication
{
    public class TwoFactorAuthenticationCommandTests
    {
        private readonly TwoFactorAuthenticationCommand _command;
        private readonly TwoFactorAuthenticationCommandHandler _handler;
        private readonly string _userId;
        private const string TOKEN = "123456";
        private readonly Mock<IIdentityService> _identityServiceMock;
        private readonly Mock<IBearerTokenService> _bearerTokenServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;

        public TwoFactorAuthenticationCommandTests()
        {
            _userId = Guid.NewGuid().ToString();

            _command = new TwoFactorAuthenticationCommand
            {
                UserId = _userId,
                Token = TOKEN
            };

            _identityServiceMock = new Mock<IIdentityService>();
            _identityServiceMock.Setup(mock => mock.Login2FAAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(
                new LoggedInResult(
                    true,
                    new AuthenticationUser
                    {
                        Id = _userId,
                    },
                    [nameof(Roles.Administrator)]
                    )
                );

            _bearerTokenServiceMock = new Mock<IBearerTokenService>();
            _bearerTokenServiceMock.Setup(mock => mock.GenerateBearerTokenAsync(
                It.IsAny<AuthenticationUser>(),
                It.IsAny<List<string>>())
            ).ReturnsAsync(nameof(TOKEN));

            _tokenServiceMock = new Mock<ITokenService>();
            _tokenServiceMock.Setup(mock => mock.GenerateTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<string>())).ReturnsAsync("RefreshToken");

            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();
            _resourceMessageProviderMock.Setup(mock => mock.GetMessage(It.IsAny<Type>(), It.IsAny<string>())).Returns("InvalidToken");

            _handler = new TwoFactorAuthenticationCommandHandler(
                _identityServiceMock.Object,
                _bearerTokenServiceMock.Object,
                _tokenServiceMock.Object,
                _resourceMessageProviderMock.Object);
        }

        [Fact]
        public async Task Handle_CorrectFlow_SuccessIsTrue()
        {
            // Act
            var result = await _handler.Handle(_command, default);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_LogInIsInvalid_ThrowsInvalidLoginException()
        {
            // Arrange
            _identityServiceMock.Setup(mock => mock.Login2FAAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(
                new LoggedInResult(false, null, null));

            // Act
            Func<Task<TwoFactorAuthenticationCommandDto>> act = () => _handler.Handle(_command, default);

            // Assert
            await act.Should().ThrowAsync<InvalidLoginException>();
        }
    }
}
