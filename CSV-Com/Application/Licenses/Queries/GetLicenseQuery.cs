using System.ComponentModel;
using Application.Common.Interfaces.CVS;
using Application.Licenses.Dtos;

namespace Application.Licenses.Queries
{
    public record GetLicenseQuery(int Id) : IRequest<LicenseDto>;

    public class GetLicenseQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetLicenseQuery, LicenseDto>
    {
        public async Task<LicenseDto> Handle(GetLicenseQuery request, CancellationToken cancellationToken)
        {
            var license = await unitOfWork.LicenseRepository
                .GetByIDAsync(request.Id, includeProperties: "Organization,LicenseHolder", cancellationToken: cancellationToken) ?? throw new NotFoundException(nameof(License), request.Id);
            return mapper.Map<LicenseDto>(license);
        }
    }
}
