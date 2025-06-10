using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Organizations.Queries.GetOrganizations
{
    [Authorize(Policy = Policies.OrganizationRead)]
    public record GetOrganizationsQuery : IRequest<IEnumerable<GetOrganizationDto>>;

    public class GetOrganizationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetOrganizationsQuery, IEnumerable<GetOrganizationDto>>
    {
        public async Task<IEnumerable<GetOrganizationDto>> Handle(GetOrganizationsQuery request, CancellationToken cancellationToken)
        {
            return (await unitOfWork.OrganizationRepository.GetAsync(includeProperties: ""))
                .AsQueryable()
                .ProjectTo<GetOrganizationDto>(mapper.ConfigurationProvider)
                .OrderBy(c => c.OrganizationName);
        }
    }
}
