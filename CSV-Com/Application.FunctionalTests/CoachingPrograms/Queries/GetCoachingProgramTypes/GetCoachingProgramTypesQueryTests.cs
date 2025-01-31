using Application.CoachingPrograms.Queries.GetCoachingProgramTypes;
using Domain.Authentication.Constants;
using Domain.CVS.Enums;

namespace Application.FunctionalTests.CoachingPrograms.Queries.GetCoachingProgramTypes
{
    public class GetCoachingProgramTypesQueryTests : BaseTestFixture
    {
        [Test]
        public async Task Handle_GetCoachingProgramTypes_ShouldReturnTypes()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var countTypes = Enum.GetValues(typeof(CoachingProgramType)).Length;
            var query = new GetCoachingProgramTypesQuery { };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull().And.NotBeEmpty().And.HaveCount(countTypes);
        }
    }
}
