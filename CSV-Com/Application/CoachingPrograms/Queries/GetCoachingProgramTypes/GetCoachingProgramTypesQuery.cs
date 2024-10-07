using AutoMapper;
using Domain.CVS.Enums;
using MediatR;

namespace Application.CoachingPrograms.Queries.GetCoachingProgramTypes
{
    public record GetCoachingProgramTypesQuery : IRequest<IEnumerable<GetCoachingProgramTypesDto>>
    {
    }

    public class GetCoachingProgramTypesQueryHandler : IRequestHandler<GetCoachingProgramTypesQuery, IEnumerable<GetCoachingProgramTypesDto>>
    {
        private readonly IMapper _mapper;

        public GetCoachingProgramTypesQueryHandler(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetCoachingProgramTypesDto>> Handle(GetCoachingProgramTypesQuery request, CancellationToken cancellationToken)
        {
            var values = Enum.GetValues(typeof(CoachingProgramType)).Cast<CoachingProgramType>();
            return values.Select(value => _mapper.Map<GetCoachingProgramTypesDto>(value));
        }
    }
}
