using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Clients.Commands.DeactivateClient
{
    [Authorize(Policy = Policies.ClientManagement)]
    public record DeactivateClientCommand : IRequest
    {
        public int Id { get; init; }
    }

    public class DeactivateClientCommandHandler : IRequestHandler<DeactivateClientCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DeactivateClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(DeactivateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await _unitOfWork.ClientRepository.GetByIDAsync(request.Id, cancellationToken);

            if (client.DeactivationDateTime != null)
            {
                throw new InvalidOperationException($"Cliënt met id '${client.Id}' is al gedeactiveerd op ${client.DeactivationDateTime} en kan dus niet opnieuw worden gedeactiveerd."); // TODO: omzetten naar result pattern
            }

            client.Deactivate(DateTime.UtcNow);

            await _unitOfWork.ClientRepository.UpdateAsync(client);
            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
