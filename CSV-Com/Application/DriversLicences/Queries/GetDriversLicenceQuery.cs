using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;
namespace Application.DriversLicences.Queries
{
    [Authorize(Policy = Policies.DriversLicenceManagement)]
    public record GetDriversLicenceQuery : IRequest<IEnumerable<DriversLicenceDto>> { }
    public class GetDriversLicenceQueryHandler : IRequestHandler<GetDriversLicenceQuery, IEnumerable<DriversLicenceDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetDriversLicenceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DriversLicenceDto>> Handle(GetDriversLicenceQuery request, CancellationToken cancellationToken)
        {
            return (await _unitOfWork.DriversLicenceRepository.GetAsync())
               .AsQueryable()
               .ProjectTo<DriversLicenceDto>(_mapper.ConfigurationProvider);
        }
    }
}
