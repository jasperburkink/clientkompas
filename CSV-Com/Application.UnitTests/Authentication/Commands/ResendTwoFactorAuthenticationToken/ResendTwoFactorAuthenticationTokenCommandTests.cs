using Application.Authentication.Commands.ResendTwoFactorAuthenticationToken;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Domain;
using Infrastructure.Identity;
using Moq;
using TestData;
using TestData.Authentication;

namespace Application.UnitTests.Authentication.Commands.ResendTwoFactorAuthenticationToken
{
    public class ResendTwoFactorAuthenticationTokenCommandTests
    {
        private readonly Mock<IIdentityService> _identityServiceMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly AuthenticationUser _user;
        private readonly IToken _token;
        private const string TokenValue = "2FATokenValue";

        public ResendTwoFactorAuthenticationTokenCommandTests()
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
                Value = TokenValue
            };

            _identityServiceMock = new Mock<IIdentityService>();
            _identityServiceMock.Setup(mock => mock.GetUserAsync(It.IsAny<string>())).ReturnsAsync(_user);
            _identityServiceMock.Setup(mock => mock.Get2FATokenAsync(It.IsAny<string>())).ReturnsAsync($"{TokenValue}_Old");

            _tokenServiceMock = new Mock<ITokenService>();
            _tokenServiceMock.Setup(mock => mock.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(_token);
            _tokenServiceMock.Setup(mock => mock.GenerateTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<string>())).ReturnsAsync($"{TokenValue}_New");
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
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_GetCurrentPendingLoginToken_GetTokenAsyncShouldBeCalledOnce()
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
            _tokenServiceMock.Verify(mock => mock.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Handle_PendingLoginTokenShouldBeVerified_ValidateTokenAsyncShouldBeCalledOnce()
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
            _tokenServiceMock.Verify(mock => mock.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Handle_New2FATokenShouldBeGenerated_Get2FATokenAsyncShouldBeCalledOnce()
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
            _identityServiceMock.Verify(mock => mock.Get2FATokenAsync(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Handle_NewPendingLoginTokenShouldBeGenerated_GenerateTokenAsyncShouldBeCalledOnce()
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
            _tokenServiceMock.Verify(mock => mock.GenerateTokenAsync(It.IsAny<AuthenticationUser>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Handle_SendEmail_SendEmailAsyncShouldBeCalledOnce()
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
            _emailServiceMock.Verify(mock => mock.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Handle_GetUserAsyncReturnsNull_ShouldThrowInvalidLoginException()
        {
            // Arrange
            var command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = _user.Id,
                TwoFactorPendingToken = nameof(TwoFactorPendingToken)
            };

            _identityServiceMock.Setup(mock => mock.GetUserAsync(It.IsAny<string>()));

            var handler = new ResendTwoFactorAuthenticationTokenCommandHandler(
                _identityServiceMock.Object,
                _tokenServiceMock.Object,
                _resourceMessageProviderMock.Object,
                _emailServiceMock.Object);

            // Act
            var act = async () => { await handler.Handle(command, default); };

            // Assert
            await act.Should().ThrowAsync<InvalidLoginException>();
        }

        [Fact]
        public async Task Handle_TwoFactorPendingTokenIsNull_ShouldThrowInvalidLoginException()
        {
            // Arrange
            var command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = _user.Id,
                TwoFactorPendingToken = nameof(TwoFactorPendingToken)
            };

            _tokenServiceMock.Setup(mock => mock.GetTokenAsync(It.IsAny<string>(), It.IsAny<string>()));

            var handler = new ResendTwoFactorAuthenticationTokenCommandHandler(
                _identityServiceMock.Object,
                _tokenServiceMock.Object,
                _resourceMessageProviderMock.Object,
                _emailServiceMock.Object);

            // Act
            var act = async () => { await handler.Handle(command, default); };

            // Assert
            await act.Should().ThrowAsync<InvalidLoginException>();
        }

        [Fact]
        public async Task Handle_UserIdsAreNotEqual_ShouldThrowInvalidLoginException()
        {
            // Arrange
            var command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = "A different userid",
                TwoFactorPendingToken = nameof(TwoFactorPendingToken)
            };

            var handler = new ResendTwoFactorAuthenticationTokenCommandHandler(
                _identityServiceMock.Object,
                _tokenServiceMock.Object,
                _resourceMessageProviderMock.Object,
                _emailServiceMock.Object);

            // Act
            var act = async () => { await handler.Handle(command, default); };

            // Assert
            await act.Should().ThrowAsync<InvalidLoginException>();
        }

        [Fact]
        public async Task Handle_ValidateTokenAsyncReturnsFalse_ShouldThrowInvalidLoginException()
        {
            // Arrange
            var command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = _user.Id,
                TwoFactorPendingToken = nameof(TwoFactorPendingToken)
            };

            _tokenServiceMock.Setup(mock => mock.ValidateTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            var handler = new ResendTwoFactorAuthenticationTokenCommandHandler(
                _identityServiceMock.Object,
                _tokenServiceMock.Object,
                _resourceMessageProviderMock.Object,
                _emailServiceMock.Object);

            // Act
            var act = async () => { await handler.Handle(command, default); };

            // Assert
            await act.Should().ThrowAsync<InvalidLoginException>();
        }

        [Fact]
        public async Task Handle_UserHasNoEmailAddress_ShouldThrowNotFoundException()
        {
            // Arrange
            ITestDataGenerator<IAuthenticationUser> testDataGenerator = new AuthenticationUserDataGenerator();
            var user = testDataGenerator.Create() as AuthenticationUser;
            user.Id = _user.Id;
            user.Email = string.Empty;

            _identityServiceMock.Setup(mock => mock.GetUserAsync(It.IsAny<string>())).ReturnsAsync(user);

            var command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = user.Id,
                TwoFactorPendingToken = nameof(TwoFactorPendingToken)
            };

            var handler = new ResendTwoFactorAuthenticationTokenCommandHandler(
                _identityServiceMock.Object,
                _tokenServiceMock.Object,
                _resourceMessageProviderMock.Object,
                _emailServiceMock.Object);

            // Act
            var act = async () => { await handler.Handle(command, default); };

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
