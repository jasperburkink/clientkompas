using Application.Authentication.Commands.RequestResetPassword;
using Domain.Authentication.Domain;
using Infrastructure.Data.Authentication;
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
            ITestDataGenerator<AuthenticationUser> testDataGenerator = new AuthenticationUserDataGenerator();
            _authenticationUser = testDataGenerator.Create();
            _authenticationUser.Email = FakerConfiguration.Faker.Person.Email;

            await AddAsync<AuthenticationUser, AuthenticationDbContext>(_authenticationUser);

            // Prepare command with the test user's email address
            _command = new RequestResetPasswordCommand
            {
                EmailAddress = _authenticationUser.Email
            };
        }

        [Test]
        public async Task Handle_CorrectEmail_ShouldReturnSuccess()
        {
            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public async Task Handle_NonExistentEmail_ShouldReturnFailureWithErrors()
        {
            // Arrange
            _command.EmailAddress = "nonexistent@example.com";

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("User not found.");
        }

        [Test]
        public void Handle_EmptyEmail_ShouldThrowArgumentException()
        {
            // Arrange
            _command.EmailAddress = string.Empty;

            // Act & Assert
            Func<Task> act = async () => await SendAsync(_command);
            act.Should().ThrowAsync<ArgumentException>().WithMessage("Value cannot be null or empty*");
        }

        [Test]
        public async Task Handle_EmailServiceFails_ShouldReturnFailureWithExceptionMessage()
        {
            // Arrange
            // Simulate an email failure by assigning an invalid email or setting a special flag on the user (depending on the implementation)
            _command.EmailAddress = "fail@example.com";

            // Act
            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Errors.Should().Contain("An error occurred while sending the reset password email.");
        }

        [Test]
        public async Task Handle_RequestTwice_ShouldReturnSuccessForBothRequests()
        {
            // Arrange
            var secondCommand = new RequestResetPasswordCommand
            {
                EmailAddress = _authenticationUser.Email
            };

            // Act
            var result1 = await SendAsync(_command);
            var result2 = await SendAsync(secondCommand);

            // Assert
            result1.Should().NotBeNull();
            result1.Success.Should().BeTrue();
            result1.Errors.Should().BeEmpty();

            result2.Should().NotBeNull();
            result2.Success.Should().BeTrue();
            result2.Errors.Should().BeEmpty();
        }
    }
}
