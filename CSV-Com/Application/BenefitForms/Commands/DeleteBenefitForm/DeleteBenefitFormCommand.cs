using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;

namespace Application.BenefitForms.Commands.DeleteBenefitForm
{
    [Authorize(Policy = Policies.BenefitFormManagement)]
    public record DeleteBenefitFormCommand : IRequest
    {
        public int Id { get; init; }
    }

    public class DeleteBenefitFormCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteBenefitFormCommand>
    {
        public async Task Handle(DeleteBenefitFormCommand request, CancellationToken cancellationToken)
        {
            var benefitForm = await unitOfWork.BenefitFormRepository.GetByIDAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(MaritalStatus), request.Id);

            var clients = await unitOfWork.ClientRepository.GetAsync(c => c.BenefitForms.Any(bf => bf.Id.Equals(request.Id)));
            if (clients.Any())
            {
                throw new DomainObjectInUseExeption(nameof(BenefitForm), request.Id, nameof(Client), clients.Select(c => (object)c.Id));
            }

            await unitOfWork.BenefitFormRepository.DeleteAsync(benefitForm);

            await unitOfWork.SaveAsync(cancellationToken);
        }
    }
}

