using Application.Common.Interfaces.CVS;
using Application.Licenses.Dtos;
using Domain.CVS.Domain;
using Domain.CVS.Enums;

namespace Application.Licenses.Commands.UpdateLicense
{
    public record UpdateLicenseCommand : IRequest<LicenseDto>
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ValidUntil { get; set; }
        public int OrganizationId { get; set; }
        public int LicenseHolderId { get; set; }
        public LicenseStatus Status { get; set; }
    }

    public class UpdateLicenseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator) : IRequestHandler<UpdateLicenseCommand, LicenseDto>
    {
        public async Task<LicenseDto> Handle(UpdateLicenseCommand request, CancellationToken cancellationToken)
        {
            var license = await unitOfWork.LicenseRepository.GetByIDAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(License), request.Id);
            var organization = await unitOfWork.OrganizationRepository.GetByIDAsync(request.OrganizationId, cancellationToken) ?? throw new NotFoundException(nameof(Organization), request.OrganizationId);
            var licenseHolder = await unitOfWork.UserRepository.GetByIDAsync(request.LicenseHolderId, cancellationToken) ?? throw new NotFoundException(nameof(User), request.LicenseHolderId);
            license.CreatedAt = request.CreatedAt;
            license.ValidUntil = request.ValidUntil;
            license.Organization = organization;
            license.LicenseHolder = licenseHolder;
            license.Status = request.Status; // Update the license status

            await unitOfWork.LicenseRepository.UpdateAsync(license, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);

            var licenseDto = mapper.Map<LicenseDto>(license);

            return licenseDto;
        }
    }
}
