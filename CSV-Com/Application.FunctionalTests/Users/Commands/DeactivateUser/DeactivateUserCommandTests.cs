using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Users.Commands.DeactivateUser;
using AutoMapper;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using Microsoft.Extensions.DependencyInjection;
using TestData;
using TestData.User;

namespace Application.FunctionalTests.Users.Commands.DeactivateUser
{
    public class DeactivateUserCommandTests : BaseTestFixture
    {
        private ITestDataGenerator<User> _testDataGeneratorUser;

        [SetUp]
        public void SetUp()
        {
            _testDataGeneratorUser = new UserDataGenerator();
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldSendReturnSuccess()
        {
            // Arrange
            var user = _testDataGeneratorUser.Create();

            await RunAsAsync(Roles.Administrator);

            await AddAsync(user);

            var command = new DeactivateUserCommand
            {
                Id = user.Id
            };

            // Act
            var result = await SendAsync(command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
        }

        [Test]
        public async Task Handle_UserDoesNotExist_ShouldReturnFailure()
        {
            // Arrange
            var user = _testDataGeneratorUser.Create();

            await RunAsAsync(Roles.Administrator);

            var command = new DeactivateUserCommand
            {
                Id = user.Id
            };

            var unitOfWork = CreateScope().ServiceProvider.GetRequiredService<IUnitOfWork>();
            var mapper = CreateScope().ServiceProvider.GetRequiredService<IMapper>();
            var emailService = CreateScope().ServiceProvider.GetRequiredService<IEmailService>();

            var handler = new DeactivateUserCommandHandler(unitOfWork, mapper, emailService);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"Gebruiker met id '${command.Id}' kan niet worden gevonden.");
        }

        [Test]
        public async Task Handle_UserIsAlreadyDeactivated_ShouldReturnFailure()
        {
            // Arrange
            var user = _testDataGeneratorUser.Create();
            user.Deactivate(DateTime.UtcNow);

            await RunAsAsync(Roles.Administrator);

            await AddAsync(user);

            var command = new DeactivateUserCommand
            {
                Id = user.Id
            };

            // Act
            var result = await SendAsync(command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.ErrorMessage.Should().Be($"Gebruiker met id '${user.Id}' is al gedeactiveerd op ${user.DeactivationDateTime} en kan dus niet opnieuw worden gedeactiveerd.");
        }

        [Test]
        public async Task Handle_UserDoesNotExist_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var user = _testDataGeneratorUser.Create();

            var command = new DeactivateUserCommand
            {
                Id = user.Id
            };

            // Act
            var act = () => SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
