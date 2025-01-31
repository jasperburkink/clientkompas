using Application.Common.Security;
using Domain.Authentication.Constants;
using Domain.CVS.Enums;

namespace Application.CoachingPrograms.Queries.GetCoachingProgramTypes
{
    [Authorize(Policy = Policies.CoachingProgramRead)]
    public record GetCoachingProgramTypesQuery : IRequest<IEnumerable<GetCoachingProgramTypesDto>>
    {
    }

    public class GetCoachingProgramTypesQueryHandler(IMapper mapper) : IRequestHandler<GetCoachingProgramTypesQuery, IEnumerable<GetCoachingProgramTypesDto>>
    {
        public async Task<IEnumerable<GetCoachingProgramTypesDto>> Handle(GetCoachingProgramTypesQuery request, CancellationToken cancellationToken)
        {
            var values = Enum.GetValues(typeof(CoachingProgramType)).Cast<CoachingProgramType>();
            return values.Select(value => mapper.Map<GetCoachingProgramTypesDto>(value));
        }
    }
}
