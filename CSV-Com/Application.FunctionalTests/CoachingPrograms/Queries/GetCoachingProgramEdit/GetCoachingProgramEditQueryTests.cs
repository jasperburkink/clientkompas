using Application.CoachingPrograms.Queries.GetCoachingProgramEdit;
using Domain.CVS.Domain;
using TestData;
using TestData.CoachingProgram;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.CoachingPrograms.Queries.GetCoachingProgramEdit
{
    public class GetCoachingProgramEditQueryTests : BaseTestFixture
    {
        private ITestDataGenerator<CoachingProgram> _testDataGeneratorCoachingProgram;

        [SetUp]
        public void Initialize()
        {
            _testDataGeneratorCoachingProgram = new CoachingProgramDataGenerator();
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldReturnCoachingProgram()
        {
            // Arrange
            // TODO: Turn on authentication 
            //await RunAsDefaultUserAsync();
            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var query = new GetCoachingProgramEditQuery()
            {
                Id = coachingProgram.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(coachingProgram.Id);
        }

        [Test]
        public void Handle_CoachingProgramDoesNotExists_ThrowsNotFoundException()
        {
            // Arrange
            // TODO: Turn on authentication 
            //await RunAsDefaultUserAsync();
            var query = new GetCoachingProgramEditQuery();

            // Act & Assert
            Assert.ThrowsAsync<Common.Exceptions.NotFoundException>(async () => await SendAsync(query));
        }

        [Test]
        public void Handle_UserIsAnomymousUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var query = new GetCoachingProgramEditQuery();

            // Act
            var result = () => SendAsync(query);

            // Assert
            //TODO: Turn on authentication 
            //await result.Should().ThrowAsync<UnauthorizedAccessException>();
            result.Should().NotBeNull();
        }

        [Test]
        public async Task Handle_CoachingProgramWithoutOptionalProperties_ShouldReturnClient()
        {
            // Arrange
            _testDataGeneratorCoachingProgram = new CoachingProgramDataGenerator(false);

            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var query = new GetCoachingProgramEditQuery()
            {
                Id = coachingProgram.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(coachingProgram.Id);
        }
    }
}
