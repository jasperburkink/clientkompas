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

    public class DeleteDriversLicenceCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteDriversLicenceCommand>
    {
        public async Task Handle(DeleteDriversLicenceCommand request, CancellationToken cancellationToken)
        {

            var driversLicence = await unitOfWork.DriversLicenceRepository.GetByIDAsync(request.Id, cancellationToken);

            driversLicence.AssertNotNull();

            await unitOfWork.DriversLicenceRepository.DeleteAsync(driversLicence);

            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
