using Application.Clients.Commands.CreateClient;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Domain.CVS.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var client = await _unitOfWork.ClientRepository.GetByIDAsync(request.ClientId, "DriversLicences", cancellationToken);
            if (client == null)
            {
                throw new NotFoundException(nameof(Client), request.ClientId);
            }

            var driversLicence = client.DriversLicences.FirstOrDefault(dl => dl.Id.Equals(request.DriversLicenceId));
            if (driversLicence == null)
            {
                throw new NotFoundException(nameof(Domain.CVS.Domain.DriversLicence), request.DriversLicenceId);
            }

            client.DriversLicences.Remove(driversLicence);

            await _unitOfWork.ClientRepository.UpdateAsync(client, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
