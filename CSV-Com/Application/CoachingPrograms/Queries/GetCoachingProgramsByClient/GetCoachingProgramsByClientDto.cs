using Application.Common.Mappings;
using AutoMapper;

namespace Application.CoachingPrograms.Queries.GetCoachingProgramsByClient
{
    public class GetCoachingProgramsByClientDto : IMapFrom<Domain.CVS.Domain.CoachingProgram>
    {
        public required int Id { get; set; }

        public required string Title { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.CVS.Domain.CoachingProgram, GetCoachingProgramsByClientDto>();
        }
    }
}
