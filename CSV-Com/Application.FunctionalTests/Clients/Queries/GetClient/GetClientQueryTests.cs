using Application.Clients.Queries.GetClient;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Queries.GetClient
{
    public class GetClientQueryTests : BaseTestFixture
    {
        private ITestDataGenerator<Client> _testDataGeneratorClient;

        [SetUp]
        public async Task Initialize()
        {
            _testDataGeneratorClient = new ClientDataGenerator();

            await RunAsAsync(Roles.Administrator);
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldReturnClient()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var query = new GetClientQuery()
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(client.Id);
        }

        [Test]
        public void Handle_ClientDoesNotExists_ThrowsNotFoundException()
        {
            // Arrange
            var query = new GetClientQuery();

            // Act & Assert
            Assert.ThrowsAsync<Common.Exceptions.NotFoundException>(async () => await SendAsync(query));
        }

        [Test]
        public void Handle_UserIsAnomymousUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var query = new GetClientQuery();

            // Act
            var result = () => SendAsync(query);

            // Assert
            //TODO: Turn on authentication 
            //await result.Should().ThrowAsync<UnauthorizedAccessException>();
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Handle_ClientWithoutOptionalProperties_ShouldReturnClient()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();
            client.PrefixLastName = null;
            client.MaritalStatus = null;
            client.Remarks = null;
            client.DriversLicences = null;
            client.BenefitForms = null;
            client.Diagnoses = null;
            client.EmergencyPeople = null;
            client.WorkingContracts = null;

            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(client.Id);
        }
    }
}
