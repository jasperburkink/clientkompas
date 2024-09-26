using Application.CoachingPrograms.Queries.GetCoachingProgram;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using TestData;
using TestData.CoachingProgram;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.CoachingPrograms.Queries.GetCoachingProgram
{
    public class GetCoachingProgramQueryTests : BaseTestFixture
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
            await RunAsAsync(Roles.Administrator);

            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var query = new GetCoachingProgramQuery()
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
        public async Task Handle_CoachingProgramDoesNotExists_ThrowsNotFoundException()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new GetCoachingProgramQuery();

            // Act & Assert
            Assert.ThrowsAsync<Common.Exceptions.NotFoundException>(async () => await SendAsync(query));
        }

        [Test]
        public async Task Handle_UserIsAnomymousUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var query = new GetCoachingProgramQuery();

            // Act
            var result = () => SendAsync(query);

            // Assert
            await result.Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Test]
        public async Task Handle_CoachingProgramWithoutOptionalProperties_ShouldReturnClient()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            _testDataGeneratorCoachingProgram = new CoachingProgramDataGenerator(false);

            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var query = new GetCoachingProgramQuery()
            {
                Id = coachingProgram.Id
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(coachingProgram.Id);
        }

        [TestCase(Roles.SystemOwner)]
        [TestCase(Roles.Licensee)]
        [TestCase(Roles.Administrator)]
        [TestCase(Roles.Coach)]
        public async Task Handle_RunAsRole_ShouldReturnCoachingProgram(string role)
        {
            // Arrange
            await RunAsAsync(role);

            var coachingProgram = _testDataGeneratorCoachingProgram.Create();

            await AddAsync(coachingProgram);

            var query = new GetCoachingProgramQuery()
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
