using Application.Authentication.Commands.RequestResetPassword;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
using TestData;
using TestData.Authentication;

namespace Application.FunctionalTests.Authentication.Commands.RequestResetPassword
{
    public class RequestResetPasswordCommandTests : BaseTestFixture
    {
        private AuthenticationUser _authenticationUser;
        private RequestResetPasswordCommand _command;

        [SetUp]
        public async Task SetUp()
        {
            ITestDataGenerator<IAuthenticationUser> testDataGenerator = new AuthenticationUserDataGenerator();
            _authenticationUser = testDataGenerator.Create() as AuthenticationUser;
            _authenticationUser.Email = FakerConfiguration.Faker.Person.Email;

            await AddAsync<AuthenticationUser, AuthenticationDbContext>(_authenticationUser);

            _command = new RequestResetPasswordCommand
            {
                EmailAddress = _authenticationUser.Email
            };
        }

        [Test]
        public async Task Handle_SuccessFlow_ResultSuccessIsTrue()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public async Task Handle_NonExistentEmail_ResultSuccessIsTrue()
        {
            // Arrange
            _command.EmailAddress = "nonexistent@example.com";

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Success.Should().BeTrue();
        }

        [Test]
        public async Task Handle_RequestTwiceForSameEmail_ResultSuccessIsTrue()
        {
            // Arrange
            var secondCommand = new RequestResetPasswordCommand
            {
                EmailAddress = _authenticationUser.Email!
            };

            // Act
            var result1 = await SendAsync(_command);
            var result2 = await SendAsync(secondCommand);

            // Assert
            result1.Success.Should().BeTrue();
            result2.Success.Should().BeTrue();
            result1.Errors.Should().BeEmpty();
            result2.Errors.Should().BeEmpty();
        }
    }
}
