using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;

namespace Application.MaritalStatuses.Commands.DeleteMaritalStatus
{
    [Authorize(Policy = Policies.MaritalStatusManagement)]
    public record DeleteMaritalStatusCommand : IRequest
    {
        public int Id { get; init; }
    }
    public class DeleteMaritalStatusCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteMaritalStatusCommand>
    {
        public async Task Handle(DeleteMaritalStatusCommand request, CancellationToken cancellationToken)
        {
            // Check if maritalstatus exists in the database
            var maritalStatus = await unitOfWork.MaritalStatusRepository.GetByIDAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(MaritalStatus), request.Id);


            // Check if there's any client that uses the maritalstatus
            var clients = await unitOfWork.ClientRepository.GetAsync(c => c.MaritalStatus.Id.Equals(request.Id));

            if (clients.Any())
            {
                throw new DomainObjectInUseExeption(nameof(MaritalStatus), request.Id, nameof(Client), clients.Select(c => (object)c.Id));
            }
            await unitOfWork.MaritalStatusRepository.DeleteAsync(maritalStatus);

            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
