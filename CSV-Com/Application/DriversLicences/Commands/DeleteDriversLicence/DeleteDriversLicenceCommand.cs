using Application.Clients.Commands.CreateClient;
using Application.Clients.Queries.GetClients;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using AutoMapper.QueryableExtensions;
using Domain.CVS.Domain;
using Domain.CVS.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Clients.Commands.DeleteClientDriversLicences
{
    public record DeleteDriversLicenceCommand : IRequest<bool>
    {
        public int DriversLicenceId { get; init; }
    }

    public class DeleteDriversLicenceCommandHandler : IRequestHandler<DeleteDriversLicenceCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDriversLicenceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteDriversLicenceCommand request, CancellationToken cancellationToken)
        {        
            var driversLicence = await _unitOfWork.DriversLicenceRepository.GetByIDAsync(request.DriversLicenceId, cancellationToken);
            if (driversLicence == null)
            {
                throw new NotFoundException(nameof(DriversLicences), request.DriversLicenceId);
            }

            await _unitOfWork.DriversLicenceRepository.DeleteAsync(driversLicence);

            await _unitOfWork.DriversLicenceRepository.UpdateAsync(driversLicence, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return true;
        }
    }
}
