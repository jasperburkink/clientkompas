using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;

namespace Application.Organizations.Commands.DeleteOrganization
{
    [Authorize(Policy = Policies.OrganizationManagement)]
    public record DeleteOrganizationCommand : IRequest
    {
        public int Id { get; init; }
    }

    public class DeleteOrganizationCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteOrganizationCommand>
    {
        public async Task Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
        {
            var org = unitOfWork.OrganizationRepository.GetByID(request.Id);
            var org2 = unitOfWork.OrganizationRepository.Get(o => o.Id == request.Id);

            var organization = await unitOfWork.OrganizationRepository.GetByIDAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Organization), request.Id);

            var hasRelatedWorkingContracts = await unitOfWork.WorkingContractRepository
                .AnyAsync(wc => wc.Organization.Id == request.Id, cancellationToken);
            if (hasRelatedWorkingContracts)
            {
                throw new ItemAlreadyExistsException("Cannot delete organization as there are related working contracts.");
            }

            await unitOfWork.OrganizationRepository.DeleteAsync(organization);

            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
