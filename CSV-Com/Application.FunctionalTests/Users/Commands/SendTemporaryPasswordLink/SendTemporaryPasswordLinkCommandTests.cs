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
        private User _cvsParentUser, _cvsUser;

        [SetUp]
        public async Task SetUp()
        {
            _testDataGeneratorAuthenticationUser = new AuthenticationUserDataGenerator();
            _testDataGeneratorCvsUser = new UserDataGenerator();
            _testDataGeneratorSendTemporaryPasswordLinkCommand = new SendTemporaryPasswordLinkCommandDataGenerator();

            _cvsParentUser = _testDataGeneratorCvsUser.Create();
            await AddAsync(_cvsParentUser);
            _cvsUser = _testDataGeneratorCvsUser.Create();
            _cvsUser.CreatedByUserId = _cvsParentUser.Id;
            await AddAsync(_cvsUser);

            var authenticationUser = _testDataGeneratorAuthenticationUser.Create();
            authenticationUser.HasTemporaryPassword = true;
            authenticationUser.CVSUserId = _cvsUser.Id;
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
            authenticationUserWithoutTempPassword.CVSUserId = _cvsUser.Id;
            await AddAsync<IAuthenticationUser, AuthenticationDbContext>(authenticationUserWithoutTempPassword);

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
        public async Task Handle_ExceedMaxSendAttempts_ShouldSendContactInformation()
        {
            // Arrange
            var authenticationUserWithMaxAttempts = _testDataGeneratorAuthenticationUser.Create();
            authenticationUserWithMaxAttempts.TemporaryPasswordTokenCount = 10;
            authenticationUserWithMaxAttempts.HasTemporaryPassword = true;
            authenticationUserWithMaxAttempts.CVSUserId = _cvsUser.Id;
            await AddAsync<IAuthenticationUser, AuthenticationDbContext>(authenticationUserWithMaxAttempts);

            var token = new TemporaryPasswordToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                IsUsed = false,
                UserId = authenticationUserWithMaxAttempts.Id,
                LoginProvider = TokenConstants.LOGINPROVIDER,
                Name = "Test",
                Value = "Test"
            };

            await AddAsync<TemporaryPasswordToken, AuthenticationDbContext>(token);

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
        public async Task Handle_ExceedMaxSendAttemptsUserWithoutCreatedByUser_ShouldReturnFailure()
        {
            // Arrange
            var cvsUser = _testDataGeneratorCvsUser.Create();
            await AddAsync(cvsUser);

            var authenticationUserWithNoCreator = _testDataGeneratorAuthenticationUser.Create();
            authenticationUserWithNoCreator.HasTemporaryPassword = true;
            authenticationUserWithNoCreator.CVSUserId = cvsUser.Id;
            authenticationUserWithNoCreator.TemporaryPasswordTokenCount = 10;
            await AddAsync<IAuthenticationUser, AuthenticationDbContext>(authenticationUserWithNoCreator);

            var token = new TemporaryPasswordToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                IsUsed = false,
                UserId = authenticationUserWithNoCreator.Id,
                LoginProvider = TokenConstants.LOGINPROVIDER,
                Name = "Test",
                Value = "Test"
            };

            await AddAsync<TemporaryPasswordToken, AuthenticationDbContext>(token);

            _command.UserId = authenticationUserWithNoCreator.Id;

            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain("User which create this user not found.");
        }

        [Test]
        public async Task Handle_UserHasNoEmail_ShouldReturnFailure()
        {
            // Arrange
            _cvsUser.EmailAddress = string.Empty;
            await UpdateAsync(_cvsUser);

            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain("No emailaddress found for this user.");
        }

        [Test]
        public async Task Handle_ExceedMaxSendAttemptsUserHasNoEmail_ShouldReturnFailure()
        {
            // Arrange
            _cvsUser.EmailAddress = string.Empty;
            await UpdateAsync(_cvsUser);

            var authenticationUserWithNoCreator = _testDataGeneratorAuthenticationUser.Create();
            authenticationUserWithNoCreator.HasTemporaryPassword = true;
            authenticationUserWithNoCreator.CVSUserId = _cvsUser.Id;
            authenticationUserWithNoCreator.TemporaryPasswordTokenCount = 10;
            await AddAsync<IAuthenticationUser, AuthenticationDbContext>(authenticationUserWithNoCreator);

            var token = new TemporaryPasswordToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                IsUsed = false,
                UserId = authenticationUserWithNoCreator.Id,
                LoginProvider = TokenConstants.LOGINPROVIDER,
                Name = "Test",
                Value = "Test"
            };

            await AddAsync<TemporaryPasswordToken, AuthenticationDbContext>(token);

            _command.UserId = authenticationUserWithNoCreator.Id;

            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain("No emailaddress found for this user.");
        }

        [Test]
        public async Task Handle_ExceedMaxSendAttemptsUserCreatedByUserHasNoEmail_ShouldReturnFailure()
        {
            // Arrange
            _cvsParentUser.EmailAddress = string.Empty;
            await UpdateAsync(_cvsParentUser);

            var authenticationUserWithNoCreator = _testDataGeneratorAuthenticationUser.Create();
            authenticationUserWithNoCreator.HasTemporaryPassword = true;
            authenticationUserWithNoCreator.CVSUserId = _cvsUser.Id;
            authenticationUserWithNoCreator.TemporaryPasswordTokenCount = 10;
            await AddAsync<IAuthenticationUser, AuthenticationDbContext>(authenticationUserWithNoCreator);

            var token = new TemporaryPasswordToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                IsUsed = false,
                UserId = authenticationUserWithNoCreator.Id,
                LoginProvider = TokenConstants.LOGINPROVIDER,
                Name = "Test",
                Value = "Test"
            };

            await AddAsync<TemporaryPasswordToken, AuthenticationDbContext>(token);

            _command.UserId = authenticationUserWithNoCreator.Id;

            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain("No emailaddress found for contactperson.");
        }

        [Test]
        public async Task Handle_TokenIsExpired_ShouldReturnSuccess()
        {
            // Arrange
            var authenticationUserWithExpiredToken = _testDataGeneratorAuthenticationUser.Create();
            authenticationUserWithExpiredToken.HasTemporaryPassword = true;
            authenticationUserWithExpiredToken.CVSUserId = _cvsUser.Id;
            await AddAsync<IAuthenticationUser, AuthenticationDbContext>(authenticationUserWithExpiredToken);

            var token = new TemporaryPasswordToken
            {
                CreatedAt = DateTime.UtcNow.AddDays(-14),
                ExpiresAt = DateTime.UtcNow.AddDays(-7),
                IsRevoked = false,
                IsUsed = false,
                UserId = authenticationUserWithExpiredToken.Id,
                LoginProvider = TokenConstants.LOGINPROVIDER,
                Name = "Test",
                Value = "Test"
            };

            await AddAsync<TemporaryPasswordToken, AuthenticationDbContext>(token);

            _command.UserId = authenticationUserWithExpiredToken.Id;

            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
        }

        [Test]
        public async Task Handle_TokenDoesNotExistsForUser_ShouldReturnFailure()
        {
            // Arrange
            var authenticationUserWithExpiredToken = _testDataGeneratorAuthenticationUser.Create();
            authenticationUserWithExpiredToken.HasTemporaryPassword = true;
            authenticationUserWithExpiredToken.CVSUserId = _cvsUser.Id;
            await AddAsync<IAuthenticationUser, AuthenticationDbContext>(authenticationUserWithExpiredToken);

            _command.UserId = authenticationUserWithExpiredToken.Id;

            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain("No valid temporary password token found for user.");
        }

        [Test]
        public async Task Handle_TokenIsRevoked_ShouldReturnFailure()
        {
            // Arrange
            var authenticationUserWithRevokedToken = _testDataGeneratorAuthenticationUser.Create();
            authenticationUserWithRevokedToken.HasTemporaryPassword = true;
            authenticationUserWithRevokedToken.CVSUserId = _cvsUser.Id;
            await AddAsync<IAuthenticationUser, AuthenticationDbContext>(authenticationUserWithRevokedToken);

            var token = new TemporaryPasswordToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = true,
                IsUsed = false,
                UserId = authenticationUserWithRevokedToken.Id,
                LoginProvider = TokenConstants.LOGINPROVIDER,
                Name = "Test",
                Value = "Test"
            };

            await AddAsync<TemporaryPasswordToken, AuthenticationDbContext>(token);

            _command.UserId = authenticationUserWithRevokedToken.Id;

            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain("No valid temporary password token found for user.");
        }

        [Test]
        public async Task Handle_TokenIsUsed_ShouldReturnFailure()
        {
            // Arrange
            var authenticationUserWithUsedToken = _testDataGeneratorAuthenticationUser.Create();
            authenticationUserWithUsedToken.HasTemporaryPassword = true;
            authenticationUserWithUsedToken.CVSUserId = _cvsUser.Id;
            await AddAsync<IAuthenticationUser, AuthenticationDbContext>(authenticationUserWithUsedToken);

            var token = new TemporaryPasswordToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                IsUsed = true,
                UserId = authenticationUserWithUsedToken.Id,
                LoginProvider = TokenConstants.LOGINPROVIDER,
                Name = "Test",
                Value = "Test"
            };

            await AddAsync<TemporaryPasswordToken, AuthenticationDbContext>(token);

            _command.UserId = authenticationUserWithUsedToken.Id;

            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain("No valid temporary password token found for user.");
        }
    }
}
