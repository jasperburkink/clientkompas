using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.CoachingPrograms.Queries.GetCoachingProgramsByClient
{
    [Authorize(Policy = Policies.CoachingProgramRead)]
    public record GetCoachingProgramsByClientQuery : IRequest<IEnumerable<GetCoachingProgramsByClientDto>>
    {
        public int ClientId { get; init; }
    }

    public class GetCoachingProgramsByClientQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetCoachingProgramsByClientQuery, IEnumerable<GetCoachingProgramsByClientDto>>
    {
        public async Task<IEnumerable<GetCoachingProgramsByClientDto>> Handle(GetCoachingProgramsByClientQuery request, CancellationToken cancellationToken)
        {
            return [.. (await unitOfWork.CoachingProgramRepository
                .GetAsync(cancellationToken: cancellationToken))
                .Where(cp => cp.ClientId == request.ClientId)
                .OrderBy(cp => cp.Title)
                .AsQueryable()
                .ProjectTo<GetCoachingProgramsByClientDto>(mapper.ConfigurationProvider)];
        }
    }
}
