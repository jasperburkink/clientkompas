using Application.Common.Interfaces.CVS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Organizations.Queries.GetOrganizations
{
    public record GetOrganizationsQuery : IRequest<IEnumerable<GetOrganizationDto>>;

    public class GetOrganizationsQueryHandler : IRequestHandler<GetOrganizationsQuery, IEnumerable<GetOrganizationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrganizationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetOrganizationDto>> Handle(GetOrganizationsQuery request, CancellationToken cancellationToken)
        {
            return (await _unitOfWork.OrganizationRepository.GetAsync(includeProperties: "WorkingContracts"))
                .AsQueryable()
                .ProjectTo<GetOrganizationDto>(_mapper.ConfigurationProvider)
                .OrderBy(c => c.OrganizationName);
        }
    }
}
