using Application.Common.Interfaces.CVS;
using Application.Licenses.Dtos;

namespace Application.Licenses.Queries
{
    public record GetAllLicensesQuery : IRequest<List<LicenseDto>>;

    public class GetAllLicensesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllLicensesQuery, List<LicenseDto>>
    {
        public async Task<List<LicenseDto>> Handle(GetAllLicensesQuery request, CancellationToken cancellationToken)
        {
            var licenses = await unitOfWork.LicenseRepository
                .GetAsync(includeProperties: "Organization,LicenseHolder", cancellationToken: cancellationToken);

            return mapper.Map<List<LicenseDto>>(licenses);
        }
    }
}
