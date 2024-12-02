using Application.Authentication.Commands.Login;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
using TestData;
using TestData.Authentication;

namespace Application.FunctionalTests.Authentication.Commands.Login
{
    public class LoginCommandDtoTests : BaseTestFixture
    {
        private string _password;
        private LoginCommand _command;
        private AuthenticationUser _authenticationUser;

        [SetUp]
        public async Task SetUp()
        {
            UseMocks = true;

            _password = PasswordGenerator.GenerateSecurePassword(16);

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

            _command = new LoginCommand
            {
                UserName = CustomWebApplicationFactoryWithMocks.AuthenticationUser.UserName,
                Password = _password
            };
        }

        [Test]
        [Ignore("Skip until mock is removed from this project.")]
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
