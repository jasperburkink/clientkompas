using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using MediatR;

namespace Application.BenefitForms.Commands.DeleteBenefitForm
{
    public record DeleteBenefitFormCommand : IRequest
    {
        public int Id { get; init; }
    }

    public class DeleteBenefitFormCommandHandler : IRequestHandler<DeleteBenefitFormCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBenefitFormCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteBenefitFormCommand request, CancellationToken cancellationToken)
        {

            // Check if maritalstatus exists in the database
            var benefitForm = await _unitOfWork.BenefitFormRepository.GetByIDAsync(request.Id, cancellationToken);
            if (benefitForm == null)
            {
                throw new NotFoundException(nameof(MaritalStatus), request.Id);
            }

            // Check if there's any client that uses the maritalstatus
            var clients = await _unitOfWork.ClientRepository.GetAsync(c => c.BenefitForm.Id.Equals(request.Id));
            if (clients.Any())
            {
                throw new DomainObjectInUseExeption(nameof(BenefitForm), request.Id, nameof(Client), clients.Select(c => (object)c.Id));
            }

            await _unitOfWork.BenefitFormRepository.DeleteAsync(benefitForm);

            await _unitOfWork.SaveAsync(cancellationToken);
        }
    }
}

