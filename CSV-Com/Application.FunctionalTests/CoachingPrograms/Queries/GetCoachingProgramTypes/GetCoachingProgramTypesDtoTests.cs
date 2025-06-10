using Application.CoachingPrograms.Queries.GetCoachingProgramTypes;
using Domain.Authentication.Constants;
using Domain.CVS.Enums;

namespace Application.FunctionalTests.CoachingPrograms.Queries.GetCoachingProgramTypes
{
    public class GetCoachingProgramTypesDtoTests : BaseTestFixture
    {
        [Test]
        public async Task Handle_GetCoachingProgramTypes_ShouldContainIds()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new GetCoachingProgramTypesQuery { };
            var ids = Enum.GetValues(typeof(CoachingProgramType)).Cast<int>();

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Select(type => type.Id).Should().BeEquivalentTo(ids);
        }

        [Test]
        public async Task Handle_GetCoachingProgramTypes_ShouldContainNames()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new GetCoachingProgramTypesQuery { };
            var names = new string[]
            {
                "Commercieel jobcoach traject",
                "Externe jobcoach",
                "Interne jobcoach",
                "Persoonsgebonden budget (pgb)",
                "Prive jobcoach traject"
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Select(type => type.Name).Should().BeEquivalentTo(names);
        }
    }
}
