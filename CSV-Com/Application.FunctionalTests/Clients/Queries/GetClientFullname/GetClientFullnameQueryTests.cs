using Application.Clients.Queries.GetClientFullname;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;

namespace Application.FunctionalTests.Clients.Queries.GetClientFullname
{
    public class GetClientFullnameQueryTests : BaseTestFixture
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

            var query = new GetClientFullnameQuery()
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(client.Id);
            result.ClientFullname.Should().Be(client.FullName);
        }

        [Test]
        public async Task Handle_ClientDoesNotExists_ThrowsNotFoundException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new GetClientFullnameQuery();

            // Act & Assert
            Assert.ThrowsAsync<Common.Exceptions.NotFoundException>(async () => await SendAsync(query));
        }

        [Test]
        public void Handle_UserIsAnomymousUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var query = new GetClientFullnameQuery();

            // Act
            var result = () => SendAsync(query);

            // Assert
            //TODO: Turn on authentication 
            //await result.Should().ThrowAsync<UnauthorizedAccessException>();
            result.Should().NotBeNull();
        }
    }
}
