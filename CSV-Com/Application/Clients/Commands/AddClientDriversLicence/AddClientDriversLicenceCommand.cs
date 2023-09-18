using Application.Clients.Commands.CreateClient;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Clients.Commands.AddClientDriversLicence
{
    public class AddClientDriversLicenceCommand : IRequest<int>
    {
        public int ClientId { get; set; }

       // public DriversLicenceEnum DriversLicence { get; set; }
    }

    public class AddClientDriversLicenceCommandHandler : IRequestHandler<AddClientDriversLicenceCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddClientDriversLicenceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(AddClientDriversLicenceCommand request, CancellationToken cancellationToken)
        {
          /*  var client = await _unitOfWork.ClientRepository.GetByIDAsync(request.ClientId, cancellationToken);

            if (client == null)
            {
                throw new NotFoundException(nameof(Client), request.ClientId);
            }

            var driversLicence = new Domain.CVS.Domain.DriversLicence() { Client = client, DriversLicenceCode = request.DriversLicence };
            client.DriversLicences.Add(driversLicence);

            await _unitOfWork.ClientRepository.UpdateAsync(client, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return driversLicence.Id;*/
          throw new NotImplementedException();
        }
    }
}