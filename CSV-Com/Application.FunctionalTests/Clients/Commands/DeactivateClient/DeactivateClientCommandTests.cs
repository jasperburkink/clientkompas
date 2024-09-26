using Application.Clients.Commands.DeactivateClient;
using Application.Common.Exceptions;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using static Application.FunctionalTests.Testing;
using ValidationException = FluentValidation.ValidationException;

namespace Application.FunctionalTests.Clients.Commands.DeactivateClient
{
    public class DeactivateClientCommandTests : BaseTestFixture
    {
        private ITestDataGenerator<Client> _testDataGeneratorClient;

        [SetUp]
        public void Initialize()
        {
            _testDataGeneratorClient = new ClientDataGenerator();
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldDeactivateClient()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var command = new DeactivateClientCommand() { Id = client.Id };

            // Act
            await SendAsync(command);

            // Assert
            var deactivatedClient = await FindAsync<Client>(client.Id);

            deactivatedClient.Should().NotBeNull();
            deactivatedClient.DeactivationDateTime.Should().NotBeNull();
            deactivatedClient.DeactivationDateTime.Value.Date.Should().Be(DateTime.Now.Date);
        }

        [Test]
        public async Task Validate_IdIs0_ThrowsValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);
            var command = new DeactivateClientCommand() { Id = 0 };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public async Task Validate_IdIsNegative_ThrowsValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);
            var command = new DeactivateClientCommand() { Id = -1000 };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public async Task Validate_ClientWithIdDoesNotExist_ThrowsValidationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var command = new DeactivateClientCommand() { Id = client.Id + 1 };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public async Task Handle_ClientIsAlreadyDeactivated_ThrowsInvalidOperationException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var client = _testDataGeneratorClient.Create();
            client.SetPrivate(c => c.DeactivationDateTime, new DateTime(2020, 04, 01));

            await AddAsync(client);

            var command = new DeactivateClientCommand() { Id = client.Id };

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => SendAsync(command));
        }

        [TestCase(Roles.SystemOwner)]
        [TestCase(Roles.Licensee)]
        [TestCase(Roles.Administrator)]
        public async Task Handle_RunAsRole_ShouldDeactivateClient(string role)
        {
            // Arrange
            await RunAsAsync(role);

            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var command = new DeactivateClientCommand() { Id = client.Id };

            // Act
            await SendAsync(command);

            // Assert
            var deactivatedClient = await FindAsync<Client>(client.Id);

            deactivatedClient.Should().NotBeNull();
            deactivatedClient!.DeactivationDateTime.Should().NotBeNull();
            deactivatedClient.DeactivationDateTime!.Value.Date.Should().Be(DateTime.Now.Date);
        }

        [TestCase(Roles.Coach)]
        public async Task Handle_RunAsRole_ShouldThrowForbiddenAccessException(string role)
        {
            // Arrange
            await RunAsAsync(role);

            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var command = new DeactivateClientCommand() { Id = client.Id };

            // Act & Assert
            Assert.ThrowsAsync<ForbiddenAccessException>(() => SendAsync(command));
        }

        [Test]
        public async Task Handle_UserIsAnomymousUser_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var command = new DeactivateClientCommand() { Id = client.Id };

            // Act & Assert
            Assert.ThrowsAsync<UnauthorizedAccessException>(() => SendAsync(command));
        }
    }
}
