using Application.Common.Interfaces.CVS;

namespace Application.Organizations.Queries.GetOrganizations
{
    public record GetOrganizationEditQuery : IRequest<GetOrganizationEditDto>
    {
        public int OrganizationId { get; init; }
    }

    internal class GetOrganizationEditQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetOrganizationEditQuery, GetOrganizationEditDto>
    {
        public async Task<GetOrganizationEditDto> Handle(GetOrganizationEditQuery request, CancellationToken cancellationToken)
        {
            // TODO: Find a better solution for including properties.
            var organization = await unitOfWork.OrganizationRepository.GetByIDAsync(request.OrganizationId, cancellationToken: cancellationToken, includeProperties: "WorkingContracts");
            return mapper.Map<GetOrganizationEditDto>(organization);
        }
    }
}
