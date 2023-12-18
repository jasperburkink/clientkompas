using Application.Common.Interfaces.CVS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Clients.Queries.SearchClients
{
    public class SearchClientsQuery : IRequest<IEnumerable<SearchClientDto>>
    {
        public string SearchTerm { get; init; }
    }

    public class SearchClientsQueryHandler : IRequestHandler<SearchClientsQuery, IEnumerable<SearchClientDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchClientsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SearchClientDto>> Handle(SearchClientsQuery request, CancellationToken cancellationToken)
        {
            return (await _unitOfWork.ClientRepository.FullTextSearch(request.SearchTerm, cancellationToken, "FullName"))
                .AsQueryable()
                .Where(c => c.DeactivationDateAndTime == null)
                .ProjectTo<SearchClientDto>(_mapper.ConfigurationProvider)
                .OrderBy(sc => sc.LastName)
                .ThenBy(sc => sc.FirstName);
        }
    }
}
