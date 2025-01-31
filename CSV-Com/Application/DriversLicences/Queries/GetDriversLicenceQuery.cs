using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;
namespace Application.DriversLicences.Queries
{
    [Authorize(Policy = Policies.DriversLicenceRead)]
    public record GetDriversLicenceQuery : IRequest<IEnumerable<DriversLicenceDto>> { }
    public class GetDriversLicenceQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetDriversLicenceQuery, IEnumerable<DriversLicenceDto>>
    {
        public async Task<IEnumerable<DriversLicenceDto>> Handle(GetDriversLicenceQuery request, CancellationToken cancellationToken)
        {
            return (await unitOfWork.DriversLicenceRepository.GetAsync())
               .AsQueryable()
               .ProjectTo<DriversLicenceDto>(mapper.ConfigurationProvider);
        }
    }
}
