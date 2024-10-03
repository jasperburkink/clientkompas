using Application.CoachingPrograms.Queries.GetCoachingProgramTypes;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Enums;

namespace Application.UnitTests.CoachingPrograms.Queries.GetCoachingProgramTypes
{
    public class GetCoachingProgramTypesQueryTests
    {
        private readonly GetCoachingProgramTypesQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public GetCoachingProgramTypesQueryTests()
        {
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _handler = new GetCoachingProgramTypesQueryHandler(_mapper);
        }

        [Fact]
        public async Task Handle_GetCoachingProgramTypes_ShouldReturnTypes()
        {
            // Arrange
            var countTypes = Enum.GetValues(typeof(CoachingProgramType)).Length;
            var query = new GetCoachingProgramTypesQuery { };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull().And.NotBeEmpty().And.HaveCount(countTypes);
        }
    }
}
