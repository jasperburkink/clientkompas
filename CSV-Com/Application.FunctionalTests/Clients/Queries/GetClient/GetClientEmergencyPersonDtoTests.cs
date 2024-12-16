using Application.Clients.Queries.GetClient;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Queries.GetClient
{
    public class GetClientEmergencyPersonDtoTests : BaseTestFixture
    {
        private ITestDataGenerator<Client> _testDataGenerator;

        [SetUp]
        public async Task Initialize()
        {
            _testDataGenerator = new ClientDataGenerator();

            await RunAsAsync(Roles.Administrator);
        }

        [Test]
        public async Task Name_IsSet_ShouldReturnName()
        {
            // Arrange
            var client = _testDataGenerator.Create();
            var expectedResult = client.EmergencyPeople.First().Name;

            await AddAsync(client);

            var query = new GetClientQuery
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
            var client = _testDataGenerator.Create();
            var expectedResult = client.EmergencyPeople.First().TelephoneNumber;

            await AddAsync(client);

            var query = new GetClientQuery
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
