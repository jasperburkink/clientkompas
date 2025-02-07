using Application.Users.Commands.CreateUser;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
using TestData;
using TestData.User.Commands;

namespace Application.FunctionalTests.Users.Commands.CreateUser
{
    public class CreateUserCommandTests : BaseTestFixture
    {
        private ITestDataGenerator<CreateUserCommand> _testDataGeneratorCreateUserCommand;
        private CreateUserCommand _command;

        [SetUp]
        public void SetUp()
        {
            _testDataGeneratorCreateUserCommand = new CreateUserCommandDataGenerator();

            // Initieer het command met geldige data voor de test
            _command = _testDataGeneratorCreateUserCommand.Create();
        }

        [Test]
        [Ignore("Skip until mock is removed from this project.")]
        public async Task Handle_CorrectFlow_ShouldCreateUser()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            // Act
            await SendAsync(_command);
            var createdUser = (await GetAsync<User>()).FirstOrDefault();

            // Assert
            createdUser.Should().NotBeNull();
            createdUser!.Id.Should().NotBe(0);
        }

        [Test]
        [Ignore("Skip until mock is removed from this project.")]
        public async Task Handle_CorrectFlow_ShouldHaveRightEmailAddress()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            // Act
            await SendAsync(_command);
            var createdUser = (await GetAsync<User>()).FirstOrDefault();

            // Assert
            createdUser!.EmailAddress.Should().Be(_command.EmailAddress);
        }

        [Test]
        [Ignore("Skip until mock is removed from this project.")]
        public async Task Handle_CorrectFlow_ShouldHaveRightRole()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            // Act
            var result = await SendAsync(_command);
            var authenticationUser = (await GetAsync<AuthenticationUser, AuthenticationDbContext>(user => user.CVSUserId == result.Value.Id)).First();

            var userRoles = await Testing.IdentityService.GetUserRolesAsync(authenticationUser.Id);

            // Assert
            userRoles.Should().Contain(_command.RoleName);
        }

        [Test]
        [Ignore("Skip until mock is removed from this project.")]
        public async Task Handle_EmailAlreadyTaken_ShouldThrowError()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            // Maak een gebruiker met hetzelfde e-mailadres
            var existingUserCommand = new CreateUserCommand
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = _command.EmailAddress,  // Zelfde e-mailadres
                TelephoneNumber = "1234567890",
                RoleName = Roles.Coach
            };
            await SendAsync(existingUserCommand);

            // Act
            var result = async () => await SendAsync(_command);

            // Assert
            await result.Should().ThrowAsync<Exception>()
                .WithMessage($"Emailaddress '{_command.EmailAddress}' is already in use.");
        }

        [Test]
        [Ignore("Skip until mock is removed from this project.")]
        public async Task Handle_TelephoneNumberAlreadyTaken_ShouldThrowError()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            // Maak een gebruiker met hetzelfde telefoonnummer
            var existingUserCommand = new CreateUserCommand
            {
                FirstName = "Jane",
                LastName = "Doe",
                EmailAddress = "jane.doe@example.com",
                TelephoneNumber = _command.TelephoneNumber,  // Zelfde telefoonnummer
                RoleName = Roles.Coach
            };
            await SendAsync(existingUserCommand);

            // Act
            var result = async () => await SendAsync(_command);

            // Assert
            await result.Should().ThrowAsync<Exception>()
                .WithMessage($"Telephonenumber '{_command.TelephoneNumber}' is already in use.");
        }

        [Test]
        [Ignore("Skip until mock is removed from this project.")]
        public async Task Handle_UserIsUnauthorized_ShouldThrowUnauthorizedAccessException()
        {
            // Act
            var result = async () => await SendAsync(_command);

            // Assert
            await result.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [TestCase(Roles.SystemOwner)]
        [TestCase(Roles.Licensee)]
        [TestCase(Roles.Administrator)]
        [TestCase(Roles.Coach)]
        [Ignore("Skip until mock is removed from this project.")]
        public async Task Handle_RunAsRole_ShouldCreateUser(string role)
        {
            // Act
            await RunAsAsync(role);

            await SendAsync(_command);
            var createdUser = (await GetAsync<User>()).FirstOrDefault();

            // Assert
            createdUser.Should().NotBeNull();
            createdUser!.Id.Should().NotBe(0);
        }

        [Test]
        [Ignore("Skip until mock is removed from this project.")]
        public async Task Handle_MultipleCommands_ShouldCreateMultipleUsers()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var command2 = new CreateUserCommand
            {
                FirstName = "Alice",
                LastName = "Smith",
                EmailAddress = "alice.smith@example.com",
                TelephoneNumber = "1234567891",
                RoleName = Roles.Coach
            };

            // Act
            await SendAsync(_command);
            await SendAsync(command2);

            // Assert
            var users = await GetAsync<User>();

            users.Should().NotBeNull().And.HaveCount(2);
        }
    }
}
