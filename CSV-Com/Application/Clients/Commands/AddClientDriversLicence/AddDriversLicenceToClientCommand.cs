using Application.Clients.Commands.CreateClient;
using Application.Clients.Queries.GetClients;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using AutoMapper;
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
    public class AddDriversLicenceToClientCommand : IRequest<ClientDto>
    {
        public int ClientId { get; set; }

        public int DriversLicenceId { get; set; }

    }

    public class AddDriversLicenceToClientCommandHandler : IRequestHandler<AddDriversLicenceToClientCommand, ClientDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AddDriversLicenceToClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ClientDto> Handle(AddDriversLicenceToClientCommand request, CancellationToken cancellationToken)
        {
            var client = await _unitOfWork.ClientRepository.GetByIDAsync(request.ClientId, cancellationToken);

            if (client == null)
            {
                throw new NotFoundException(nameof(Client), request.ClientId);
            }
            var driversLicence = await _unitOfWork.DriversLicenceRepository.GetByIDAsync(request.DriversLicenceId, cancellationToken);

            if (driversLicence == null)
            {
                throw new NotFoundException(nameof(Client), request.DriversLicenceId);
            }

            throw new NotImplementedException();
        }
    }
}