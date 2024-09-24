using Application.Authentication.Commands.Login;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using TestData;
using TestData.Authentication;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Authentication.Commands
{
    public class LoginCommandTests : BaseTestFixture
    {
        private string _password;
        private LoginCommand _command;
        private ITestDataGenerator<AuthenticationUser> _testDataGeneratorAuthenticationUser;

        [SetUp]
        public async Task SetUp()
        {
            _testDataGeneratorAuthenticationUser = new AuthenticationUserDataGenerator();
            var authenticationUser = _testDataGeneratorAuthenticationUser.Create();

            _password = PasswordGenerator.GenerateSecurePassword(16);

            await RunAsUserAsync(authenticationUser.UserName, _password, Roles.Coach);

            _command = new LoginCommand
            {
                UserName = authenticationUser.UserName,
                Password = _password
            };
        }

        [Ignore("Logging in via tests is not working yet.")]
        //[Test]
        public async Task Handle_CorrectFlow_ShouldBeLoggedIn()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Ignore("Logging in via tests is not working yet.")]
        //[Test]
        public async Task Handle_InvalidLoginData_ShouldNoBeLoggedIn()
        {
            // Arrange
            var command = _command with
            {
                Password = "InvalidPassword"
            };

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
        }
    }
}
