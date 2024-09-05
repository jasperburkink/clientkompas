using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;

namespace Application.Clients.Commands.DeleteClientDriversLicence
{
    public record DeleteClientDriversLicenceCommand : IRequest
    {
        public int ClientId { get; init; }

        public int DriversLicenceId { get; init; }
    }

    public class DeleteClientDriversLicenceCommandHandler : IRequestHandler<DeleteClientDriversLicenceCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteClientDriversLicenceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteClientDriversLicenceCommand request, CancellationToken cancellationToken)
        {
            var client = await _unitOfWork.ClientRepository.GetByIDAsync(request.ClientId, "DriversLicences", cancellationToken)
                ?? throw new NotFoundException(nameof(Client), request.ClientId);

            var driversLicence = client.DriversLicences.FirstOrDefault(dl => dl.Id.Equals(request.DriversLicenceId))
                ?? throw new NotFoundException(nameof(DriversLicence), request.DriversLicenceId);

            client.DriversLicences.Remove(driversLicence);

            await _unitOfWork.ClientRepository.UpdateAsync(client, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
