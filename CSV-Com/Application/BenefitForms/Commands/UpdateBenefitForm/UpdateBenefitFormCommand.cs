using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BenefitForms.Commands.UpdateBenefitForm
{
    public record UpdateBenefitFormCommand : IRequest<BenefitForm>
    {
        public int Id { get; init; }
        public string Name { get; set; }
    }
    public class UpdateBenefitFormCommandHandler : IRequestHandler<UpdateBenefitFormCommand, BenefitForm>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateBenefitFormCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BenefitForm> Handle(UpdateBenefitFormCommand request, CancellationToken cancellationToken)
        {
            var benefitForm = await _unitOfWork.BenefitFormRepository.GetByIDAsync(request.Id, cancellationToken);
            if (benefitForm == null)
            {
                throw new NotFoundException(nameof(BenefitForm), request.Id);
            }
            benefitForm.Name = request.Name;
            await _unitOfWork.SaveAsync(cancellationToken);
            return benefitForm;
        }
    }
}
