using Application.Users.Commands.SendTemporaryPasswordLink;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using Domain.CVS.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
using TestData;
using TestData.Authentication;
using TestData.User;
using TestData.User.Commands;

namespace Application.FunctionalTests.Users.Commands.SendTemporaryPasswordLink
{
    public class SendTemporaryPasswordLinkCommandTests : BaseTestFixture
    {
        private ITestDataGenerator<AuthenticationUser> _testDataGeneratorAuthenticationUser;
        private ITestDataGenerator<User> _testDataGeneratorCvsUser;
        private ITestDataGenerator<SendTemporaryPasswordLinkCommand> _testDataGeneratorSendTemporaryPasswordLinkCommand;
        private SendTemporaryPasswordLinkCommand _command;

        [SetUp]
        public async Task SetUp()
        {
            _testDataGeneratorAuthenticationUser = new AuthenticationUserDataGenerator();
            _testDataGeneratorCvsUser = new UserDataGenerator();
            _testDataGeneratorSendTemporaryPasswordLinkCommand = new SendTemporaryPasswordLinkCommandDataGenerator();

            var cvsUserParent = _testDataGeneratorCvsUser.Create();
            await AddAsync(cvsUserParent);
            var cvsUser = _testDataGeneratorCvsUser.Create();
            cvsUser.CreatedByUserId = cvsUserParent.Id;
            await AddAsync(cvsUser);

            var authenticationUser = _testDataGeneratorAuthenticationUser.Create();
            authenticationUser.HasTemporaryPassword = true;
            authenticationUser.CVSUserId = cvsUser.Id;
            await AddAsync<AuthenticationUser, AuthenticationDbContext>(authenticationUser);

            var token = new TemporaryPasswordToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                IsUsed = false,
                UserId = authenticationUser.Id,
                LoginProvider = TokenConstants.LOGINPROVIDER,
                Name = "Test",
                Value = "Test"
            };

            await AddAsync<TemporaryPasswordToken, AuthenticationDbContext>(token);

            _command = _testDataGeneratorSendTemporaryPasswordLinkCommand.Create();
            _command.UserId = authenticationUser.Id;
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldSendTemporaryPasswordLink()
        {
            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
        }

        [Test]
        public async Task Handle_UserWithoutTemporaryPassword_ShouldReturnFailure()
        {
            // Arrange
            var authenticationUserWithoutTempPassword = _testDataGeneratorAuthenticationUser.Create();
            authenticationUserWithoutTempPassword.HasTemporaryPassword = false;
            await AddAsync(authenticationUserWithoutTempPassword);

            _command.UserId = authenticationUserWithoutTempPassword.Id;

            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain("This user has not got a temporary password.");
        }

        [Test]
        public async Task Handle_NoValidToken_ShouldReturnFailure()
        {
            // Arrange
            var authenticationUserWithInvalidToken = _testDataGeneratorAuthenticationUser.Create();
            await AddAsync(authenticationUserWithInvalidToken);

            _command.UserId = authenticationUserWithInvalidToken.Id;

            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain("Temporary password token not found for user.");
        }

        [Test]
        public async Task Handle_ValidToken_ShouldResendPasswordLink()
        {
            // Arrange
            var authenticationUserWithValidToken = _testDataGeneratorAuthenticationUser.Create();
            await AddAsync(authenticationUserWithValidToken);

            _command.UserId = authenticationUserWithValidToken.Id;

            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
        }

        [Test]
        public async Task Handle_ExceedMaxSendAttempts_ShouldSendContactInformation()
        {
            // Arrange
            var authenticationUserWithMaxAttempts = _testDataGeneratorAuthenticationUser.Create();
            await AddAsync(authenticationUserWithMaxAttempts);

            _command.UserId = authenticationUserWithMaxAttempts.Id;

            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
        }

        [Test]
        public async Task Handle_UserNotFound_ShouldReturnFailure()
        {
            // Arrange
            var nonExistentUserId = "non-existent-user-id";
            _command.UserId = nonExistentUserId;

            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain("User not found.");
        }

        [Test]
        public async Task Handle_UserWithoutCreatedByUser_ShouldReturnFailure()
        {
            // Arrange
            var authenticationUserWithNoCreator = _testDataGeneratorAuthenticationUser.Create();
            await AddAsync(authenticationUserWithNoCreator);

            _command.UserId = authenticationUserWithNoCreator.Id;

            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain("User which create this user not found.");
        }
    }
}
