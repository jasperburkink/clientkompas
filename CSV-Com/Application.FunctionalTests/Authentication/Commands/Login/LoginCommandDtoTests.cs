using Application.Authentication.Commands.Login;
using TestData;

namespace Application.FunctionalTests.Authentication.Commands.Login
{
    public class LoginCommandDtoTests : BaseTestFixture
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
        public async Task Handle_CorrectFlow_SuccessShouldBeTrue()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Success.Should().BeTrue();
        }

        [Test]
        [Ignore("Cannot mock identityservice login multiple times for login. Skip until mock is removed from this project.")]
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

        [TearDown]
        public void TearDown() => UseMocks = false;
    }
}
