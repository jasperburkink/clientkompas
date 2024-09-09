using Application.Authentication.Commands.Login;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using TestData;
using TestData.Authentication;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Authentication.Commands
{
    public class LoginCommandDtoTests : BaseTestFixture
    {
        private string _password;
        private LoginCommand _command;
        private ITestDataGenerator<AuthenticationUser> _testDataGeneratorAuthenticationUser;

        [SetUp]
        public async Task SetUp()
        {
            _testDataGeneratorAuthenticationUser = new AuthenticationUserDataGenerator();
            var authenticationUser = _testDataGeneratorAuthenticationUser.Create();

            _password = PasswordGenerator.GenerateSecurePassword(8);

            //await AddAsync<AuthenticationUser, AuthenticationDbContext>(authenticationUser);
            await RunAsUserAsync(authenticationUser.UserName, _password, [Roles.Coach]);

            _command = new LoginCommand
            {
                UserName = authenticationUser.UserName,
                Password = _password
            };
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldBeLoggedIn()
        {
            // Arrange
            //await RunAsUserAsync();


            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }
    }
}
