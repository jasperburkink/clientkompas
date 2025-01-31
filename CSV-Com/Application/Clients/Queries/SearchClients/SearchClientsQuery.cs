using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Clients.Queries.SearchClients
{
    [Authorize(Policy = Policies.ClientRead)]
    public class SearchClientsQuery : IRequest<IEnumerable<SearchClientDto>>
    {
        public string SearchTerm { get; init; }
    }

    public class SearchClientsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<SearchClientsQuery, IEnumerable<SearchClientDto>>
    {
        public async Task<IEnumerable<SearchClientDto>> Handle(SearchClientsQuery request, CancellationToken cancellationToken)
        {
            return [.. (await unitOfWork.ClientRepository.FullTextSearch(request.SearchTerm, cancellationToken, client => client.FullName))
                .AsQueryable()
                .Where(c => c.DeactivationDateTime == null)
                .ProjectTo<SearchClientDto>(mapper.ConfigurationProvider)
                .OrderBy(sc => sc.LastName)
                .ThenBy(sc => sc.FirstName)];
        }
    }
}
