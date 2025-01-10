using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Organizations.Queries.SearchOrganizations
{
    [Authorize(Policy = Policies.OrganizationRead)]
    public class SearchOrganizationQuery : IRequest<IEnumerable<SearchOrganizationDto>>
    {
        public string SearchTerm { get; init; }
    }

    public class SearchOrganizationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<SearchOrganizationQuery, IEnumerable<SearchOrganizationDto>>
    {
        public async Task<IEnumerable<SearchOrganizationDto>> Handle(SearchOrganizationQuery request, CancellationToken cancellationToken)
        {
            return (await unitOfWork.OrganizationRepository.FullTextSearch(request.SearchTerm, cancellationToken, organization => organization.OrganizationName))
                .AsQueryable()
                .ProjectTo<SearchOrganizationDto>(mapper.ConfigurationProvider)
                .OrderBy(sc => sc.OrganizationName);
        }
    }
}
