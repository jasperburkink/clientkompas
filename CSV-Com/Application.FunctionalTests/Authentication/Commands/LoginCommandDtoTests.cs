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
        public async Task Handle_CorrectFlow_SuccessShouldBeTrue()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Success.Should().BeTrue();
        }

        [Ignore("Logging in via tests is not working yet.")]
        //[Test]
        public async Task Handle_InvalidLoginData_SuccessShouldBeFalse()
        {
            // Arrange
            var command = new LoginCommand
            {
                UserName = "Test",
                Password = "Invalid"
            };

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Success.Should().BeFalse();
        }
    }
}
