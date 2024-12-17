using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Licenses.Dtos;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using MediatR;

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

    public class CreateLicenseCommandHandler : IRequestHandler<CreateLicenseCommand, LicenseDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateLicenseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<LicenseDto> Handle(CreateLicenseCommand request, CancellationToken cancellationToken)
        {
            var organization = await _unitOfWork.OrganizationRepository.GetByIDAsync(request.OrganizationId, cancellationToken)
                ?? throw new NotFoundException(nameof(Organization), request.OrganizationId);
            var licenseHolder = await _unitOfWork.UserRepository.GetByIDAsync(request.LicenseHolderId, cancellationToken)
                ?? throw new NotFoundException(nameof(User), request.LicenseHolderId);

            var license = new License
            {
                CreatedAt = request.CreatedAt,
                ValidUntil = request.ValidUntil,
                Organization = organization,
                LicenseHolder = licenseHolder,
                Status = request.Status
            };

            await _unitOfWork.LicenseRepository.InsertAsync(license, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            var licenseDto = _mapper.Map<LicenseDto>(license);

            return licenseDto;
        }
    }
}
