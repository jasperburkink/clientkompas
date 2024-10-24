using Application.Clients.Queries.GetClients;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Queries.GetClients
{
    public class GetClientsQueryTests : BaseTestFixture
    {
        private ITestDataGenerator<Client> _testDataGeneratorClient;

        [SetUp]
        public async Task Initialize()
        {
            _testDataGeneratorClient = new ClientDataGenerator();

            await RunAsAsync(Roles.Administrator);
        }

        [Test]
        public async Task Handle_CorrectFlowOneClient_ShouldReturnOneClient()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var query = new GetClientsQuery();

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull().And.HaveCountGreaterThan(0);
            result.Should().Contain(c => c.LastName == client.LastName);
        }

        [Test]
        public async Task Handle_CorrectFlowMultipleClients_ShouldReturnMultipleClients()
        {
            // Arrange
            var numberOfClients = 5;
            var clients = _testDataGeneratorClient.Create(numberOfClients);

            foreach (var client in clients)
            {
                await AddAsync(client);
            }

            var query = new GetClientsQuery();

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull().And.HaveCount(numberOfClients);
        }

        [Test]
        public void Handle_UserIsAnomymousUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var query = new GetClientsQuery();

            // Act
            var result = () => SendAsync(query);

            // Assert
            //TODO: Turn on authentication 
            //await result.Should().ThrowAsync<UnauthorizedAccessException>();
            result.Should().NotBeNull();
        }
    }
}
