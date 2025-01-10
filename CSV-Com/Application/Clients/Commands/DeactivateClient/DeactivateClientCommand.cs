using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Clients.Commands.DeactivateClient
{
    [Authorize(Policy = Policies.DeactiveClient)]
    public record DeactivateClientCommand : IRequest
    {
        public int Id { get; init; }
    }

    public class DeactivateClientCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<DeactivateClientCommand>
    {
        public async Task Handle(DeactivateClientCommand request, CancellationToken cancellationToken)
        {
            var client = await unitOfWork.ClientRepository.GetByIDAsync(request.Id, cancellationToken);

            if (client == null)
            {
                throw new NotFoundException(nameof(client), request.Id);
            }

            if (client.DeactivationDateTime != null)
            {
                throw new InvalidOperationException($"Cliënt met id '${client.Id}' is al gedeactiveerd op ${client.DeactivationDateTime} en kan dus niet opnieuw worden gedeactiveerd."); // TODO: omzetten naar result pattern
            }

            client.Deactivate(DateTime.UtcNow);

            await unitOfWork.ClientRepository.UpdateAsync(client);
            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
