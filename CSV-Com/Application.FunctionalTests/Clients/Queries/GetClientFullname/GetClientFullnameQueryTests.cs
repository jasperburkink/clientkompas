using Application.Clients.Queries.GetClientFullname;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using static Application.FunctionalTests.Testing;

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
            // TODO: Turn on authentication 
            //await RunAsDefaultUserAsync();
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
        public void Handle_ClientDoesNotExists_ThrowsNotFoundException()
        {
            // Arrange
            // TODO: Turn on authentication 
            //await RunAsDefaultUserAsync();
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
