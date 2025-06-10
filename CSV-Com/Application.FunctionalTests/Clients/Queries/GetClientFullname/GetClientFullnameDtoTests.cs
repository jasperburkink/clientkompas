using Application.Clients.Queries.GetClientFullname;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;

namespace Application.FunctionalTests.Clients.Queries.GetClientFullname
{
    public class GetClientFullnameDtoTests : BaseTestFixture
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
            await RunAsAsync(Roles.Administrator);

            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.Id;

            var query = new GetClientFullnameQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Id.Should().Be(expectedResult);
        }

        [Test]
        public async Task Fullname_IsSet_ShouldReturnFullname()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var expectedResult = client.FullName;

            var query = new GetClientFullnameQuery
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.ClientFullname.Should().Be(expectedResult);
        }
    }
}
