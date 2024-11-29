using Application.Authentication.Commands.ResendTwoFactorAuthenticationToken;
using Application.Common.Exceptions;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
using TestData;
using TestData.Authentication;

namespace Application.FunctionalTests.Authentication.Commands.ResendTwoFactorAuthenticationToken
{
    public class ResendTwoFactorAuthenticationTokenCommandTests : BaseTestFixture
    {
        private ResendTwoFactorAuthenticationTokenCommand _command;
        private AuthenticationUser _authenticationUser;

        [SetUp]
        public async Task SetUp()
        {
            ITestDataGenerator<AuthenticationUser> testDataGeneratorAuthenticationUser = new AuthenticationUserDataGenerator();
            _authenticationUser = testDataGeneratorAuthenticationUser.Create();

            var initialPassword = Utils.GeneratePassword();
            var id = await CreateUserAsync(_authenticationUser.Email!, initialPassword);

            _authenticationUser.Id = id;

            var token = new TwoFactorPendingToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(1),
                IsRevoked = false,
                IsUsed = false,
                UserId = _authenticationUser.Id,
                LoginProvider = "Test",
                Name = "TestTwoFactorPendingToken",
                Value = "Test"
            };
            await AddAsync<TwoFactorPendingToken, AuthenticationDbContext>(token);

            _command = new ResendTwoFactorAuthenticationTokenCommand
            {
                UserId = _authenticationUser.Id,
                TwoFactorPendingToken = token.Value
            };
        }

        [Test]
        public async Task Handle_CorrectFlow_SuccessIsTrue()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task Handle_UserIdIsWrong_ShouldThrowInvalidLoginException()
        {
            // Arrange
            var command = _command with
            {
                UserId = Guid.NewGuid().ToString(),
            };

            // Act
            var act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<InvalidLoginException>();
        }

        [Test]
        public async Task Handle_PendingTokenIsWrong_ShouldThrowInvalidLoginException()
        {
            // Arrange
            var command = _command with
            {
                TwoFactorPendingToken = "WrongValue"
            };

            // Act
            var act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<InvalidLoginException>();
        }

        [Test]
        public async Task Handle_PendingTokenIsRevoked_ShouldThrowInvalidLoginException()
        {
            // Arrange
            var token = new TwoFactorPendingToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(1),
                IsRevoked = true,
                IsUsed = false,
                UserId = _authenticationUser.Id,
                LoginProvider = "Test",
                Name = "NewTestTwoFactorPendingToken",
                Value = "NewTest"
            };

            await AddAsync<TwoFactorPendingToken, AuthenticationDbContext>(token);

            var command = _command with
            {
                TwoFactorPendingToken = token.Value
            };

            // Act
            var act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<InvalidLoginException>();
        }

        [Test]
        public async Task Handle_PendingTokenIsUsed_ShouldThrowInvalidLoginException()
        {
            // Arrange
            var token = new TwoFactorPendingToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(1),
                IsRevoked = false,
                IsUsed = true,
                UserId = _authenticationUser.Id,
                LoginProvider = "Test",
                Name = "NewTestTwoFactorPendingToken",
                Value = "NewTest"
            };

            await AddAsync<TwoFactorPendingToken, AuthenticationDbContext>(token);

            var command = _command with
            {
                TwoFactorPendingToken = token.Value
            };

            // Act
            var act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<InvalidLoginException>();
        }

        [Test]
        public async Task Handle_PendingToken_ShouldThrowInvalidLoginException()
        {
            // Arrange
            var token = new TwoFactorPendingToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddMinutes(1),
                IsRevoked = false,
                IsUsed = true,
                UserId = _authenticationUser.Id,
                LoginProvider = "Test",
                Name = "NewTestTwoFactorPendingToken",
                Value = "NewTest"
            };

            await AddAsync<TwoFactorPendingToken, AuthenticationDbContext>(token);

            var command = _command with
            {
                TwoFactorPendingToken = token.Value
            };

            // Act
            var act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<InvalidLoginException>();
        }

        [Test]
        public async Task Handle_CorrectFlow_SuccessIsTrue()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert

        }
    }
}
