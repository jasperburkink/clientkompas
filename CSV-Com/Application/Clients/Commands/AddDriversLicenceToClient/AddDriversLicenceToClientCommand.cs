using Application.Clients.Dtos;
using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;

namespace Application.Clients.Commands.AddDriversLicenceToClient
{
    [Authorize(Policy = Policies.ClientManagement)]
    public class AddDriversLicenceToClientCommand : IRequest<ClientDto>
    {
        public int ClientId { get; set; }

        public int DriversLicenceId { get; set; }

    }

    public class AddDriversLicenceToClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AddDriversLicenceToClientCommand, ClientDto>
    {
        public async Task<ClientDto> Handle(AddDriversLicenceToClientCommand request, CancellationToken cancellationToken)
        {
            var client = await unitOfWork.ClientRepository.GetByIDAsync(request.ClientId, cancellationToken) ?? throw new NotFoundException(nameof(Client), request.ClientId);
            var driversLicence = await unitOfWork.DriversLicenceRepository.GetByIDAsync(request.DriversLicenceId, cancellationToken) ?? throw new NotFoundException(nameof(Client), request.DriversLicenceId);

            throw new NotImplementedException();
        }
    }
}
