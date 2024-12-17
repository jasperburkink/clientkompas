using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using MediatR;

namespace Application.Licenses.Commands.BlockLicense
{
    public record BlockLicenseCommand(int Id) : IRequest<Unit>;

    public class BlockLicenseCommandHandler : IRequestHandler<BlockLicenseCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public BlockLicenseCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(BlockLicenseCommand request, CancellationToken cancellationToken)
        {
            var license = await _unitOfWork.LicenseRepository.GetByIDAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(License), request.Id);

            if (license.Status == LicenseStatus.Blocked)
            {
                throw new InvalidOperationException($"License with ID {request.Id} is already blocked.");
            }

            if (license.Status == LicenseStatus.Blocked)
            {
                throw new InvalidOperationException("License is already blocked.");
            }

            license.Status = LicenseStatus.Blocked;

            await _unitOfWork.LicenseRepository.UpdateAsync(license, cancellationToken);
            await _unitOfWork.SaveAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
