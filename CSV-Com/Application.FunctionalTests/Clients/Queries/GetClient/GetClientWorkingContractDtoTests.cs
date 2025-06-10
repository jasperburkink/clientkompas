using Application.Clients.Queries.GetClient;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Queries.GetClient
{
    internal class GetClientWorkingContractDtoTests : BaseTestFixture
    {
        private ITestDataGenerator<Client> _testDataGenerator;

        [SetUp]
        public async Task Initialize()
        {
            _testDataGenerator = new ClientDataGenerator();

            await RunAsAsync(Roles.Administrator);
        }

        [Test]
        public async Task Id_IsSet_ShouldReturnId()
        {
            // Arrange
            var client = _testDataGenerator.Create();

            await AddAsync(client);

            var expectedResult = client.WorkingContracts.First().Id;

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.WorkingContracts.First().Id.Should().Be(expectedResult);
        }

        [Test]
        public async Task Function_IsSet_ShouldReturnFunction()
        {
            // Arrange
            var client = _testDataGenerator.Create();
            var expectedResult = client.WorkingContracts.First().Function;

            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.WorkingContracts.First().Function.Should().Be(expectedResult);
        }

        [Test]
        public async Task ContractType_IsSet_ShouldReturnContractType()
        {
            // Arrange
            var client = _testDataGenerator.Create();
            var expectedResult = (int)client.WorkingContracts.First().ContractType;

            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.WorkingContracts.First().ContractType.Should().Be(expectedResult);
        }

        [Test]
        public async Task FromDate_IsSet_ShouldReturnFromDate()
        {
            // Arrange
            var client = _testDataGenerator.Create();
            var expectedResult = client.WorkingContracts.First().FromDate;

            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.WorkingContracts.First().FromDate.Should().Be(expectedResult);
        }

        [Test]
        public async Task ToDate_IsSet_ShouldReturnToDate()
        {
            // Arrange
            var client = _testDataGenerator.Create();
            var expectedResult = client.WorkingContracts.First().ToDate;

            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.WorkingContracts.First().ToDate.Should().Be(expectedResult);
        }

        [Test]
        public async Task OrganizationName_IsSet_ShouldReturnOrganizationName()
        {
            // Arrange
            var client = _testDataGenerator.Create();
            var expectedResult = client.WorkingContracts.First().Organization.OrganizationName;

            await AddAsync(client);

            var query = new GetClientQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.WorkingContracts.First().OrganizationName.Should().Be(expectedResult);
        }
    }
}
