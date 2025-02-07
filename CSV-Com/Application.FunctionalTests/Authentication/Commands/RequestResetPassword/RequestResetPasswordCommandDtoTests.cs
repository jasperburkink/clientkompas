using Application.Authentication.Commands.RequestResetPassword;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
using TestData;
using TestData.Authentication;

namespace Application.FunctionalTests.Authentication.Commands.RequestResetPassword
{
    public class RequestResetPasswordCommandDtoTests : BaseTestFixture
    {
        private AuthenticationUser _authenticationUser;
        private RequestResetPasswordCommand _command;

        [SetUp]
        public async Task SetUp()
        {
            ITestDataGenerator<IAuthenticationUser> testDataGeneratorAuthenticationUser = new AuthenticationUserDataGenerator();
            _authenticationUser = testDataGeneratorAuthenticationUser.Create() as AuthenticationUser;
            await AddAsync<AuthenticationUser, AuthenticationDbContext>(_authenticationUser);

            _command = new RequestResetPasswordCommand
            {
                EmailAddress = _authenticationUser.Email
            };
        }

        [Test]
        public async Task Handle_CorrectFlow_SuccessShouldBeTrue()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task Handle_CorrectFlow_ErrorsShouldBeEmpty()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Errors.Should().BeNullOrEmpty();
        }

        [Test]
        public async Task Handle_InvalidEmail_SuccessShouldBeTrue()
        {
            // Arrange
            _command.EmailAddress = "invalid@example.com";

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task Handle_InvalidEmail_ErrorsShouldContainNoErrorMessages()
        {
            // Arrange
            _command.EmailAddress = "invalid@example.com";

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Errors.Should().BeEmpty();
        }
    }
}
