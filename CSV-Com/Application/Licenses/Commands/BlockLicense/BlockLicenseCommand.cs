using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Domain.CVS.Enums;

namespace Application.Licenses.Commands.BlockLicense
{
    public record BlockLicenseCommand(int Id) : IRequest<Unit>;

    public class BlockLicenseCommandHandler(IUnitOfWork unitOfWork, IMediator mediator) : IRequestHandler<BlockLicenseCommand, Unit>
    {
        public async Task<Unit> Handle(BlockLicenseCommand request, CancellationToken cancellationToken)
        {
            var license = await unitOfWork.LicenseRepository.GetByIDAsync(request.Id, cancellationToken) ?? throw new NotFoundException(nameof(License), request.Id);

            if (license.Status == LicenseStatus.Blocked)
            {
                throw new InvalidOperationException($"License with ID {request.Id} is already blocked.");
            }

            if (license.Status == LicenseStatus.Blocked)
            {
                throw new InvalidOperationException("License is already blocked.");
            }

            license.Status = LicenseStatus.Blocked;

            await unitOfWork.LicenseRepository.UpdateAsync(license, cancellationToken);
            await unitOfWork.SaveAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
