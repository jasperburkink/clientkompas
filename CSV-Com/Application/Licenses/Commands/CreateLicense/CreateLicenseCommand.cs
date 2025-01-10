using Application.Common.Interfaces.CVS;
using Application.Licenses.Dtos;
using Domain.CVS.Domain;
using Domain.CVS.Enums;

namespace Application.Licenses.Commands.CreateLicense
{
    public record CreateLicenseCommand : IRequest<LicenseDto>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime ValidUntil { get; set; }
        public int OrganizationId { get; set; }
        public int LicenseHolderId { get; set; }
        public LicenseStatus Status { get; set; }
    }

    public class CreateLicenseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator) : IRequestHandler<CreateLicenseCommand, LicenseDto>
    {
        public async Task<LicenseDto> Handle(CreateLicenseCommand request, CancellationToken cancellationToken)
        {
            var organization = await unitOfWork.OrganizationRepository.GetByIDAsync(request.OrganizationId, cancellationToken)
                ?? throw new NotFoundException(nameof(Organization), request.OrganizationId);
            var licenseHolder = await unitOfWork.UserRepository.GetByIDAsync(request.LicenseHolderId, cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.LicenseHolderId);

            var license = new License
            {
                CreatedAt = request.CreatedAt,
                ValidUntil = request.ValidUntil,
                Organization = organization,
                LicenseHolder = licenseHolder,
                Status = request.Status
            };

            await unitOfWork.LicenseRepository.InsertAsync(license, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);

            var licenseDto = mapper.Map<LicenseDto>(license);

            return licenseDto;
        }
    }
}
