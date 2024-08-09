using Application.CoachingPrograms.Queries.GetCoachingProgramsByClient;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using TestData.CoachingProgram;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.CoachingPrograms.Queries.GetCoachingProgramsByClient
{
    public class GetCoachingProgramsByClientQueryTests
    {
        private ITestDataGenerator<Client> _testDataGeneratorClient;
        private ITestDataGenerator<CoachingProgram> _testDataGeneratorCoachingProgram;

        [SetUp]
        public void Initialize()
        {
            _testDataGeneratorClient = new ClientDataGenerator();
            _testDataGeneratorCoachingProgram = new CoachingProgramDataGenerator();
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldReturnCoachingPrograms()
        {
            // Arrange
            // TODO: Turn on authentication 
            //await RunAsDefaultUserAsync();
            var client = _testDataGeneratorClient.Create();
            var coachingPrograms = _testDataGeneratorCoachingProgram.Create(5).ToList();
            for (var i = 0; i < coachingPrograms.Count(); i++)
            {
                coachingPrograms[i].ClientId = client.Id;
                coachingPrograms[i].Client.Id = client.Id;
            }
            asdsad
            await AddAsync(client);

            var query = new GetCoachingProgramsByClientQuery()
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
