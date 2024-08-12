using Application.CoachingPrograms.Queries.GetCoachingProgramsByClient;
using Domain.CVS.Domain;
using TestData;
using TestData.CoachingProgram;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.CoachingPrograms.Queries.GetCoachingProgramsByClient
{
    public class GetCoachingProgramsByClientDtoTests : BaseTestFixture
    {
        private ITestDataGenerator<CoachingProgram> _testDataGeneratorCoachingProgram;

        [SetUp]
        public void Initialize()
        {
            _testDataGeneratorCoachingProgram = new CoachingProgramDataGenerator();
        }

        [Test]
        public async Task Id_IsSet_ShouldReturnId()
        {
            // Arrange            
            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var expectedResult = coachingProgram.Id;

            var query = new GetCoachingProgramsByClientQuery
            {
                ClientId = coachingProgram.ClientId
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.First().Id.Should().Be(expectedResult);
        }

        [Test]
        public async Task Title_IsSet_ShouldReturnTitle()
        {
            // Arrange            
            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var expectedResult = coachingProgram.Title;

            var query = new GetCoachingProgramsByClientQuery
            {
                ClientId = coachingProgram.ClientId
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.First().Title.Should().Be(expectedResult);
        }
    }
}
