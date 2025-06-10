using Application.Clients.Queries.GetClientEdit;
using Domain.Authentication.Constants;
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
            await RunAsAsync(Roles.Administrator);

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
        public async Task Handle_ClientDoesNotExists_ThrowsNotFoundException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new GetClientEditQuery();

            // Act & Assert
            Assert.ThrowsAsync<Common.Exceptions.NotFoundException>(async () => await SendAsync(query));
        }

        [Test]
        public async Task Handle_UserIsAnomymousUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var query = new GetClientEditQuery();

            // Act
            var result = () => SendAsync(query);

            // Assert
            await result.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Test]
        public async Task Handle_ClientWithoutOptionalProperties_ShouldReturnClient()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

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

        [TestCase(Roles.SystemOwner)]
        [TestCase(Roles.Licensee)]
        [TestCase(Roles.Administrator)]
        [TestCase(Roles.Coach)]
        public async Task Handle_RunAsRole_ShouldReturnClient(string role)
        {
            // Arrange
            await RunAsAsync(role);

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
    }
}
