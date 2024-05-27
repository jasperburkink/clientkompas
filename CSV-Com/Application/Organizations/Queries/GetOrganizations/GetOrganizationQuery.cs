using Application.Common.Interfaces.CVS;
using AutoMapper;
using MediatR;

namespace Application.Organizations.Queries.GetOrganizations
{
    public record GetOrganizationQuery : IRequest<GetOrganizationDto>
    {
        public int OrganizationId { get; init; }
    }

    public class GetOrganizationQueryHandler : IRequestHandler<GetOrganizationQuery, GetOrganizationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrganizationQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetOrganizationDto> Handle(GetOrganizationQuery request, CancellationToken cancellationToken)
        {
            var organization = await _unitOfWork.OrganizationRepository.GetByIDAsync(request.OrganizationId, cancellationToken: cancellationToken, includeProperties: "WorkingContracts");
            return _mapper.Map<GetOrganizationDto>(organization);
        }
    }
}
