using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Licenses.Dtos;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using MediatR;

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

    public class UpdateLicenseCommandHandler : IRequestHandler<UpdateLicenseCommand, LicenseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UpdateLicenseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<LicenseDto> Handle(UpdateLicenseCommand request, CancellationToken cancellationToken)
        {
            var license = await _unitOfWork.LicenseRepository.GetByIDAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(License), request.Id);
            var organization = await _unitOfWork.OrganizationRepository.GetByIDAsync(request.OrganizationId, cancellationToken) ?? throw new NotFoundException(nameof(Organization), request.OrganizationId);
            var licenseHolder = await _unitOfWork.UserRepository.GetByIDAsync(request.LicenseHolderId, cancellationToken) ?? throw new NotFoundException(nameof(User), request.LicenseHolderId);
            license.CreatedAt = request.CreatedAt;
            license.ValidUntil = request.ValidUntil;
            license.Organization = organization;
            license.LicenseHolder = licenseHolder;
            license.Status = request.Status; // Update the license status

            await _unitOfWork.LicenseRepository.UpdateAsync(license, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            var licenseDto = _mapper.Map<LicenseDto>(license);

            return licenseDto;
        }
    }
}
