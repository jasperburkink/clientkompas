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
    public record DeleteDriversLicenceCommand : IRequest
    {
        public int DriversLicenceId { get; init; }
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
            var driversLicence = await _unitOfWork.DriversLicenceRepository.GetByIDAsync(request.DriversLicenceId, "DriversLicences", cancellationToken);
            if (driversLicence == null)
            {
                throw new NotFoundException(nameof(DriversLicence), request.DriversLicenceId);
            }

            /* var driversLicence = client.DriversLicences.FirstOrDefault(dl => dl.Id.Equals(request.DriversLicenceId));
           if (driversLicence == null)
           {
               throw new NotFoundException(nameof(Domain.CVS.Domain.DriversLicence), request.DriversLicenceId);
           }*/
            await _unitOfWork.DriversLicenceRepository.DeleteAsync(driversLicence);

            await _unitOfWork.DriversLicenceRepository.UpdateAsync(driversLicence, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
