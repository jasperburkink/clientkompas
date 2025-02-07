using Application.Authentication.Commands.TwoFactorAuthentication;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;

namespace Application.FunctionalTests.Authentication.Commands.TwoFactorAuthentication
{
    public class TwoFactorAuthenticationCommandDtoTests : BaseTestFixture
    {
        private TwoFactorAuthenticationCommand _command;

        [SetUp]
        public async Task SetUp()
        {
            UseMocks = true;

            AddAsync<AuthenticationUser, AuthenticationDbContext>(CustomWebApplicationFactoryWithMocks.AuthenticationUser).GetAwaiter().GetResult();

            _command = new TwoFactorAuthenticationCommand
            {
                UserId = CustomWebApplicationFactoryWithMocks.AuthenticationUser.Id,
                Token = "321654"
            };
        }

        [Test]
        [Ignore("Skip until mock is removed from this project.")]
        public async Task Handle_Success_ShouldBeTrue()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Test]
        [Ignore("Skip until mock is removed from this project.")]
        public async Task Handle_BearerToken_ShouldBeSet()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.BearerToken.Should().NotBeNullOrEmpty();
        }

        [Test]
        [Ignore("Skip until mock is removed from this project.")]
        public async Task Handle_RefreshToken_ShouldBeSet()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.RefreshToken.Should().NotBeNullOrEmpty();
        }

        [TearDown]
        public void TearDown() => UseMocks = false;
    }
}
