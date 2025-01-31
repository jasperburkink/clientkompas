using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Organizations.Queries.GetOrganizations
{
    [Authorize(Policy = Policies.OrganizationRead)]
    public record GetOrganizationQuery : IRequest<GetOrganizationDto>
    {
        public int OrganizationId { get; init; }
    }

    public class GetOrganizationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetOrganizationQuery, GetOrganizationDto>
    {
        public async Task<GetOrganizationDto> Handle(GetOrganizationQuery request, CancellationToken cancellationToken)
        {
            var organization = await unitOfWork.OrganizationRepository.GetByIDAsync(request.OrganizationId, cancellationToken: cancellationToken, includeProperties: "");
            return mapper.Map<GetOrganizationDto>(organization);
        }
    }
}
