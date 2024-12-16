using Application.Authentication.Commands.ResendTwoFactorAuthenticationToken;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
using TestData;
using TestData.Authentication;

namespace Application.FunctionalTests.Authentication.Commands.ResendTwoFactorAuthenticationToken
{
    public class ResendTwoFactorAuthenticationTokenCommandDtoTests : BaseTestFixture
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
        public async Task Handle_Success_IsTrue()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task Handle_UserId_IsSet()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.UserId.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Handle_TwoFactorPendingToken_IsSet()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.TwoFactorPendingToken.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Handle_ExpiresAt_IsSet()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.ExpiresAt.Should().NotBeNull();
        }

        [Test]
        public async Task Handle_ExpiresAt_IsInTheFuture()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
        }
    }
}
