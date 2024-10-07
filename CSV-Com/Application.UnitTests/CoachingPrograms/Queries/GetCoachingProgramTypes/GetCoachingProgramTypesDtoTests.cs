using Application.CoachingPrograms.Queries.GetCoachingProgramTypes;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Enums;

namespace Application.UnitTests.CoachingPrograms.Queries.GetCoachingProgramTypes
{
    public class GetCoachingProgramTypesDtoTests
    {
        private readonly GetCoachingProgramTypesQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public GetCoachingProgramTypesDtoTests()
        {
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _handler = new GetCoachingProgramTypesQueryHandler(_mapper);
        }

        [Fact]
        public async Task Handle_GetCoachingProgramTypes_ShouldContainIds()
        {
            // Arrange
            var query = new GetCoachingProgramTypesQuery { };
            var ids = Enum.GetValues(typeof(CoachingProgramType)).Cast<int>();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Select(type => type.Id).Should().BeEquivalentTo(ids);
        }

        [Fact]
        public async Task Handle_GetCoachingProgramTypes_ShouldContainNames()
        {
            // Arrange
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
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Select(type => type.Name).Should().BeEquivalentTo(names);
        }
    }
}
