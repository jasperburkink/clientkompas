using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Application.Extensions;
using Domain.Authentication.Constants;

namespace Application.DriversLicences.Commands.DeleteDriversLicence
{
    [Authorize(Policy = Policies.DriversLicenceManagement)]
    public record DeleteDriversLicenceCommand : IRequest
    {
        public int Id { get; init; }
    }

    public class DeleteDriversLicenceCommandHandler : IRequestHandler<DeleteDriversLicenceCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDriversLicenceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteDriversLicenceCommand request, CancellationToken cancellationToken)
        {

            var driversLicence = await _unitOfWork.DriversLicenceRepository.GetByIDAsync(request.Id, cancellationToken);

            driversLicence.AssertNotNull();

            await _unitOfWork.DriversLicenceRepository.DeleteAsync(driversLicence);

            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
