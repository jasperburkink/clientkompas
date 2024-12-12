using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Organizations.Queries.SearchOrganizations
{
    [Authorize(Policy = Policies.OrganizationManagement)]
    public class SearchOrganizationQuery : IRequest<IEnumerable<SearchOrganizationDto>>
    {
        public string SearchTerm { get; init; }
    }

    public class SearchOrganizationsQueryHandler : IRequestHandler<SearchOrganizationQuery, IEnumerable<SearchOrganizationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchOrganizationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SearchOrganizationDto>> Handle(SearchOrganizationQuery request, CancellationToken cancellationToken)
        {
            return (await _unitOfWork.OrganizationRepository.FullTextSearch(request.SearchTerm, cancellationToken, organization => organization.OrganizationName))
                .AsQueryable()
                .ProjectTo<SearchOrganizationDto>(_mapper.ConfigurationProvider)
                .OrderBy(sc => sc.OrganizationName);
        }
    }
}
