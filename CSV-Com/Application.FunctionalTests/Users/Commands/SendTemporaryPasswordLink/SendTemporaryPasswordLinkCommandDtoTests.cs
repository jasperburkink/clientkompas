using Application.Users.Commands.SendTemporaryPasswordLink;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
using TestData;
using TestData.Authentication;
using TestData.User;
using TestData.User.Commands;

namespace Application.FunctionalTests.Users.Commands.SendTemporaryPasswordLink
{
    public class SendTemporaryPasswordLinkCommandDtoTests : BaseTestFixture
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
            result.Value.Should().NotBeNull();
            result.Value!.UserId.Should().Be(_cvsUser.Id);
        }

        [Test]
        public async Task Handle_UserWithoutTemporaryPassword_DtoShouldBeNull()
        {
            // Arrange
            var authenticationUserWithoutTempPassword = _testDataGeneratorAuthenticationUser.Create();
            authenticationUserWithoutTempPassword.HasTemporaryPassword = false;
            authenticationUserWithoutTempPassword.CVSUserId = _cvsUser.Id;
            await AddAsync<AuthenticationUser, AuthenticationDbContext>(authenticationUserWithoutTempPassword);

            _command.UserId = authenticationUserWithoutTempPassword.Id;

            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Value.Should().BeNull();
        }
    }
}
