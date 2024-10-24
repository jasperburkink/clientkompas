using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.CoachingPrograms.Queries.GetCoachingProgramsByClient
{
    [Authorize(Policy = Policies.CoachingProgramManagement)]
    public record GetCoachingProgramsByClientQuery : IRequest<IEnumerable<GetCoachingProgramsByClientDto>>
    {
        public int ClientId { get; init; }
    }

    public class GetCoachingProgramsByClientQueryHandler : IRequestHandler<GetCoachingProgramsByClientQuery, IEnumerable<GetCoachingProgramsByClientDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;



        public GetCoachingProgramsByClientQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetCoachingProgramsByClientDto>> Handle(GetCoachingProgramsByClientQuery request, CancellationToken cancellationToken)
        {
            return (await _unitOfWork.CoachingProgramRepository
                .GetAsync(cancellationToken: cancellationToken))
                .Where(cp => cp.ClientId == request.ClientId)
                .OrderBy(cp => cp.Title)
                .AsQueryable()
                .ProjectTo<GetCoachingProgramsByClientDto>(_mapper.ConfigurationProvider)
                .ToList();
        }
    }
}
