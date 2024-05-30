using Application.Common.Interfaces.CVS;
using AutoMapper;
using MediatR;

namespace Application.Organizations.Queries.GetOrganizations
{
    public record GetOrganizationEditQuery : IRequest<GetOrganizationEditDto>
    {
        public int OrganizationId { get; init; }
    }

    internal class GetOrganizationEditQueryHandler : IRequestHandler<GetOrganizationEditQuery, GetOrganizationEditDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrganizationEditQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetOrganizationEditDto> Handle(GetOrganizationEditQuery request, CancellationToken cancellationToken)
        {
            // TODO: Find a better solution for including properties.
            var organization = await _unitOfWork.OrganizationRepository.GetByIDAsync(request.OrganizationId, cancellationToken: cancellationToken, includeProperties: "WorkingContracts");
            return _mapper.Map<GetOrganizationEditDto>(organization);
        }
    }
}
