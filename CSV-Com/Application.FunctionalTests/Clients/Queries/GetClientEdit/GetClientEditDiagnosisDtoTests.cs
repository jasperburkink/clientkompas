using Application.Clients.Queries.GetClientEdit;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Queries.GetClientEdit
{
    public class GetClientEditDiagnosisDtoTests : BaseTestFixture
    {
        private ITestDataGenerator<Client> _testDataGeneratorClient;

        [SetUp]
        public void Initialize()
        {
            _testDataGeneratorClient = new ClientDataGenerator();
        }

        [Test]
        public async Task Id_IsSet_ShouldReturnId()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.Diagnoses.First().Id;

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Diagnoses.First().Id.Should().Be(expectedResult);
        }

        [Test]
        public async Task Name_IsSet_ShouldReturnName()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.Diagnoses.First().Name;

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Diagnoses.First().Name.Should().Be(expectedResult);
        }
    }
}
