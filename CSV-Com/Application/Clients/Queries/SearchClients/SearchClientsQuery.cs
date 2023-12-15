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
            return (await _unitOfWork.ClientRepository.FullTextSearch(request.SearchTerm, cancellationToken))
                .AsQueryable()
                .ProjectTo<SearchClientDto>(_mapper.ConfigurationProvider)
                .OrderBy(sc => sc.LastName)
                .ThenBy(sc => sc.FirstName);

            return (await _unitOfWork.ClientRepository.GetAsync())
                .AsQueryable()
                .Where(c => // TODO: Welke stringcomparison is het beste voor het vergelijken van strings in een collectie?
                    string.IsNullOrEmpty(request.SearchTerm) || // TODO: Toevoegen van unittest voor deze situatie, wanneer er geen zoekterm is meegegeven.                    
                    c.LastName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.FirstName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.PrefixLastName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.Initials.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
                .ProjectTo<SearchClientDto>(_mapper.ConfigurationProvider)
                .OrderBy(sc => sc.LastName)
                .ThenBy(sc => sc.FirstName);
        }
    }
}
