using Application.Clients.Queries.GetClientEdit;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Queries.GetClientEdit
{
    public class GetClientEditWorkingContractDtoTests : BaseTestFixture
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

            var expectedResult = client.WorkingContracts.First().Id;

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.WorkingContracts.First().Id.Should().Be(expectedResult);
        }

        [Test]
        public async Task ContractType_IsSet_ShouldReturnContractType()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = (int)client.WorkingContracts.First().ContractType;

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.WorkingContracts.First().ContractType.Should().Be(expectedResult);
        }

        [Test]
        public async Task Function_IsSet_ShouldReturnFunction()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.WorkingContracts.First().Function;

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.WorkingContracts.First().Function.Should().Be(expectedResult);
        }

        [Test]
        public async Task Organization_IsSet_ShouldReturnOrganization()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.WorkingContracts.First().Organization.Id;

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.WorkingContracts.First().OrganizationId.Should().Be(expectedResult);
        }

        [Test]
        public async Task ToDate_IsSet_ShouldReturnToDate()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.WorkingContracts.First().ToDate;

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.WorkingContracts.First().ToDate.Should().Be(expectedResult);
        }

        [Test]
        public async Task FromDate_IsSet_ShouldReturnFromDate()
        {
            // Arrange            
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.WorkingContracts.First().FromDate;

            var query = new GetClientEditQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.WorkingContracts.First().FromDate.Should().Be(expectedResult);
        }
    }
}
