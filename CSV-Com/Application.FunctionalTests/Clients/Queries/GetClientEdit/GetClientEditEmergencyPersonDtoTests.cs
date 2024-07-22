using Application.Clients.Queries.GetClientEdit;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Queries.GetClientEdit
{
    public class GetClientEditEmergencyPersonDtoTests : BaseTestFixture
    {
        private ITestDataGenerator<Client> _testDataGeneratorClient;

        [SetUp]
        public void Initialize()
        {
            _testDataGeneratorClient = new ClientDataGenerator();
        }

        [Test]
        public async Task Name_IsSet_ShouldReturnName()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.EmergencyPeople.First().Name;

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.EmergencyPeople.First().Name.Should().Be(expectedResult);
        }

        [Test]
        public async Task TelephoneNumber_IsSet_ShouldReturnTelephoneNumber()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.EmergencyPeople.First().TelephoneNumber;

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.EmergencyPeople.First().TelephoneNumber.Should().Be(expectedResult);
        }
    }
}
