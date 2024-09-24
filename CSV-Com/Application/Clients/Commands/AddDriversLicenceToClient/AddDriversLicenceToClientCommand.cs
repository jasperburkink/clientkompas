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
            var client = await _unitOfWork.ClientRepository.GetByIDAsync(request.ClientId, cancellationToken) ?? throw new NotFoundException(nameof(Client), request.ClientId);
            var driversLicence = await _unitOfWork.DriversLicenceRepository.GetByIDAsync(request.DriversLicenceId, cancellationToken) ?? throw new NotFoundException(nameof(Client), request.DriversLicenceId);

            throw new NotImplementedException();
        }
    }
}
