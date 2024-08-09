using Application.Common.Interfaces.CVS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.CoachingPrograms.Queries.GetCoachingProgramsByClient
{
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
