using Application.Authentication.Commands.Login;
using TestData;

namespace Application.FunctionalTests.Authentication.Commands.Login
{
    public class LoginCommandTests : BaseTestFixture
    {
        private string _password;
        private LoginCommand _command;

        [SetUp]
        public async Task SetUp()
        {
            UseMocks = true;

            _password = PasswordGenerator.GenerateSecurePassword(16);

            _command = new LoginCommand
            {
                UserName = CustomWebApplicationFactoryWithMocks.AuthenticationUser.UserName,
                Password = _password
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

        [Test]
        [Ignore("Cannot mock identityservice login multiple times for login. Skip until mock is removed from this project.")]
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

        [Test]
        [Ignore("Cannot mock identityservice login multiple times for login. Skip until mock is removed from this project.")]
        public async Task Handle_LoggingInAnonymous_ShouldBeLoggedIn()
        {
            // Arrange
            await ResetState();
            await RunAsAsync("Anonymous");

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [TearDown]
        public void TearDown() => UseMocks = false;
    }
}
