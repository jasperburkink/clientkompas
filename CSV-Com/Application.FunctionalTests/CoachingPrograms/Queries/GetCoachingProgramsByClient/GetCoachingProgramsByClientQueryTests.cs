using Application.CoachingPrograms.Queries.GetCoachingProgramsByClient;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;
using TestData.CoachingProgram;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.CoachingPrograms.Queries.GetCoachingProgramsByClient
{
    public class GetCoachingProgramsByClientQueryTests : BaseTestFixture
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
            await RunAsAsync(Roles.Administrator);

            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var coachingPrograms = _testDataGeneratorCoachingProgram.Create(5).ToList();
            for (var i = 0; i < coachingPrograms.Count(); i++)
            {
                coachingPrograms[i].ClientId = client.Id;
                coachingPrograms[i].Client = null;

                await AddAsync(coachingPrograms[i]);
            }

            var query = new GetCoachingProgramsByClientQuery()
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(coachingPrograms.Count());
        }

        [Test]
        public async Task Handle_UserIsAnomymousUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var coachingPrograms = _testDataGeneratorCoachingProgram.Create(5).ToList();
            for (var i = 0; i < coachingPrograms.Count(); i++)
            {
                coachingPrograms[i].ClientId = client.Id;
                coachingPrograms[i].Client = null;

                await AddAsync(coachingPrograms[i]);
            }

            var query = new GetCoachingProgramsByClientQuery()
            {
                ClientId = client.Id
            };

            // Act
            var result = () => SendAsync(query);

            // Assert
            await result.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [TestCase(Roles.SystemOwner)]
        [TestCase(Roles.Licensee)]
        [TestCase(Roles.Administrator)]
        [TestCase(Roles.Coach)]
        public async Task Handle_RunAsRole_ShouldReturnCoachingPrograms(string role)
        {
            // Arrange
            await RunAsAsync(role);

            var client = _testDataGeneratorClient.Create();

            await AddAsync(client);

            var coachingPrograms = _testDataGeneratorCoachingProgram.Create(5).ToList();
            for (var i = 0; i < coachingPrograms.Count(); i++)
            {
                coachingPrograms[i].ClientId = client.Id;
                coachingPrograms[i].Client = null;

                await AddAsync(coachingPrograms[i]);
            }

            var query = new GetCoachingProgramsByClientQuery()
            {
                ClientId = client.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(coachingPrograms.Count());
        }
    }
}
