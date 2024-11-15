using Application.Authentication.Commands.TwoFactorAuthentication;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;

namespace Application.FunctionalTests.Authentication.Commands.TwoFactorAuthentication
{
    public class TwoFactorAuthenticationCommandTests : BaseTestFixture
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
        public async Task Handle_CorrectFlow_ShouldBeLoggedIn()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        // TODO: Add additional tests when https://sbict.atlassian.net/browse/CVS-578 is solved and mocks are removed.

        [TearDown]
        public void TearDown() => UseMocks = false;
    }
}
