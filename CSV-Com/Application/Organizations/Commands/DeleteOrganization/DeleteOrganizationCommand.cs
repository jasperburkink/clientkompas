using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using MediatR;

namespace Application.Organizations.Commands.DeleteOrganization
{
    public record DeleteOrganizationCommand : IRequest
    {
        public int Id { get; init; }
    }

    public class DeleteOrganizationCommandHandler : IRequestHandler<DeleteOrganizationCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteOrganizationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
        {
            var org = _unitOfWork.OrganizationRepository.GetByID(request.Id);
            var org2 = _unitOfWork.OrganizationRepository.Get(o => o.Id == request.Id);

            var organization = await _unitOfWork.OrganizationRepository.GetByIDAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(Organization), request.Id);

            var hasRelatedWorkingContracts = await _unitOfWork.WorkingContractRepository
                .AnyAsync(wc => wc.Organization.Id == request.Id, cancellationToken);
            if (hasRelatedWorkingContracts)
            {
                throw new ItemAlreadyExistsException("Cannot delete organization as there are related working contracts.");
            }

            await _unitOfWork.OrganizationRepository.DeleteAsync(organization);

            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}
