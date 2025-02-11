using Application.Authentication.Commands.ResendTwoFactorAuthenticationToken;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using Infrastructure.Identity;
using Moq;
using TestData;
using TestData.Authentication;

namespace Application.UnitTests.Authentication.Commands.ResendTwoFactorAuthenticationToken
{
    public class ResendTwoFactorAuthenticationTokenCommandDtoTests
    {
        private readonly Mock<IIdentityService> _identityServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly AuthenticationUser _user;
        private readonly IAuthenticationToken _token;
        private const string TokenValueOld = "2FATokenValue_Old";
        private const string TokenValueNew = "2FATokenValue_New";

        public ResendTwoFactorAuthenticationTokenCommandDtoTests()
        {
            ITestDataGenerator<IAuthenticationUser> testDataGenerator = new AuthenticationUserDataGenerator();
            _user = testDataGenerator.Create() as AuthenticationUser;

            _token = new TwoFactorPendingToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsRevoked = false,
                IsUsed = false,
                UserId = _user.Id,
                Value = TokenValueOld
            };

            _identityServiceMock = new Mock<IIdentityService>();
            _identityServiceMock.Setup(mock => mock.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _identityServiceMock.Setup(mock => mock.Get2FATokenAsync(It.IsAny<string>())).ReturnsAsync(TokenValueOld);

            _tokenServiceMock = new Mock<ITokenService>();
            _tokenServiceMock.Setup(mock => mock.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_token);
            _tokenServiceMock.Setup(mock => mock.GenerateTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<string>())).ReturnsAsync(TokenValueNew);
            _tokenServiceMock.Setup(mock => mock.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();
            _resourceMessageProviderMock.Setup(mock => mock.GetMessage(It.IsAny<Type>(), It.IsAny<string>())).Returns("ResourceValue");

            _emailServiceMock = new Mock<IEmailService>();
            _emailServiceMock.Setup(mock => mock.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task Handle_CorrectFlow_SuccessIsTrue()
        {
            // Arrange
            var command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = _user.Id,
                TwoFactorPendingToken = nameof(TwoFactorPendingToken)
            };

            var handler = new ResendTwoFactorAuthenticationTokenCommandHandler(
                _identityServiceMock.Object,
                _tokenServiceMock.Object,
                _resourceMessageProviderMock.Object,
                _emailServiceMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_CorrectFlow_UserIdIsSet()
        {
            // Arrange
            var command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = _user.Id,
                TwoFactorPendingToken = nameof(TwoFactorPendingToken)
            };

            var handler = new ResendTwoFactorAuthenticationTokenCommandHandler(
                _identityServiceMock.Object,
                _tokenServiceMock.Object,
                _resourceMessageProviderMock.Object,
                _emailServiceMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.UserId.Should().Be(_user.Id);
        }

        [Fact]
        public async Task Handle_CorrectFlow_TwoFactorPendingTokenIsSet()
        {
            // Arrange
            var command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = _user.Id,
                TwoFactorPendingToken = nameof(TwoFactorPendingToken)
            };

            var handler = new ResendTwoFactorAuthenticationTokenCommandHandler(
                _identityServiceMock.Object,
                _tokenServiceMock.Object,
                _resourceMessageProviderMock.Object,
                _emailServiceMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.TwoFactorPendingToken.Should().Be(TokenValueNew);
        }

        [Fact]
        public async Task Handle_CorrectFlow_ExpiresAtIsSet()
        {
            // Arrange
            var command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = _user.Id,
                TwoFactorPendingToken = nameof(TwoFactorPendingToken)
            };

            var now = DateTime.UtcNow.Add(TwoFactorPendingTokenConstants.TOKEN_TIMEOUT);

            var handler = new ResendTwoFactorAuthenticationTokenCommandHandler(
                _identityServiceMock.Object,
                _tokenServiceMock.Object,
                _resourceMessageProviderMock.Object,
                _emailServiceMock.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.ExpiresAt.Should().BeCloseTo(now, TimeSpan.FromSeconds(1));
        }
    }
}
