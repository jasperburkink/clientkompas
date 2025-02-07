using Application.Common.Exceptions;
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
    internal class SendTemporaryPasswordLinkCommandValidatorTests : BaseTestFixture
    {
        private ITestDataGenerator<IAuthenticationUser> _testDataGeneratorAuthenticationUser;
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

            var authenticationUser = _testDataGeneratorAuthenticationUser.Create() as AuthenticationUser;
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
        public async Task Handle_CorrectFlow_NoValidationErrors()
        {
            // Act
            await RunAsAsync(Roles.Administrator);

            var result = await SendAsync(_command);

            // Assert
            result.Should().NotBeNull();
            result.Errors.Should().BeEmpty();
        }

        [Test]
        public async Task Handle_UseridIsNull_ShouldThrowValidationException()
        {
            // Arrange
            _command.UserId = null;

            // Act
            await RunAsAsync(Roles.Administrator);
            var act = async () => await SendAsync(_command);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task Handle_UserIdIsEmptyString_ShouldThrowValidationException()
        {
            // Arrange
            _command.UserId = string.Empty;

            // Act
            await RunAsAsync(Roles.Administrator);
            var act = async () => await SendAsync(_command);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
