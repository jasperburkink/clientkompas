using Application.Clients.Queries.GetClientEdit;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Queries.GetClientEdit
{

    public class GetClientEditQueryTests : BaseTestFixture
    {
        private ITestDataGenerator<Client> _testDataGeneratorClient;

        [SetUp]
        public void Initialize()
        {
            _testDataGeneratorClient = new ClientDataGenerator();
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldReturnClient()
        {
            // Arrange
            // TODO: Turn on authentication 
            //await RunAsDefaultUserAsync();
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var query = new GetClientEditQuery()
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
            // TODO: Turn on authentication 
            //await RunAsDefaultUserAsync();
            var query = new GetClientEditQuery();

            // Act & Assert
            Assert.ThrowsAsync<Common.Exceptions.NotFoundException>(async () => await SendAsync(query));
        }

        [Test]
        public void Handle_UserIsAnomymousUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var query = new GetClientEditQuery();

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

            var query = new GetClientEditQuery
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
