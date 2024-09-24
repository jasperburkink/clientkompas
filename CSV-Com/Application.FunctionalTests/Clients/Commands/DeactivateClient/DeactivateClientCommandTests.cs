using Application.Clients.Commands.DeactivateClient;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using FluentValidation;
using TestData;
using TestData.Client;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Commands.DeactivateClient
{
    public class DeactivateClientCommandTests : BaseTestFixture
    {
        private ITestDataGenerator<Client> _testDataGeneratorClient;

        [SetUp]
        public async Task Initialize()
        {
            _testDataGeneratorClient = new ClientDataGenerator();

            await RunAsAsync(Roles.Administrator);
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldDeactivateClient()
        {
            // Arrange
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
        public void Validate_IdIs0_ThrowsValidationException()
        {
            // Arrange
            var command = new DeactivateClientCommand() { Id = 0 };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Validate_IdIsNegative_ThrowsValidationException()
        {
            // Arrange
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
            var client = _testDataGeneratorClient.Create();
            client.SetPrivate(c => c.DeactivationDateTime, new DateTime(2020, 04, 01));

            await AddAsync(client);

            var command = new DeactivateClientCommand() { Id = client.Id };

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => SendAsync(command));
        }
    }
}
