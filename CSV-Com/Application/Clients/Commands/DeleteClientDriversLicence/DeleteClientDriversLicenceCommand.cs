using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;

namespace Application.Clients.Commands.DeleteClientDriversLicence
{
    [Authorize(Policy = Policies.ClientManagement)]
    public record DeleteClientDriversLicenceCommand : IRequest
    {
        public int ClientId { get; init; }

        public int DriversLicenceId { get; init; }
    }

    public class DeleteClientDriversLicenceCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteClientDriversLicenceCommand>
    {
        public async Task Handle(DeleteClientDriversLicenceCommand request, CancellationToken cancellationToken)
        {
            var client = await unitOfWork.ClientRepository.GetByIDAsync(request.ClientId, "DriversLicences", cancellationToken)
                ?? throw new NotFoundException(nameof(Client), request.ClientId);

            var driversLicence = client.DriversLicences.FirstOrDefault(dl => dl.Id.Equals(request.DriversLicenceId))
                ?? throw new NotFoundException(nameof(DriversLicence), request.DriversLicenceId);

            client.DriversLicences.Remove(driversLicence);

            await unitOfWork.ClientRepository.UpdateAsync(client, cancellationToken);

            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
