using Application.BenefitForms.Queries.GetBenefitForm;
using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using Domain.CVS.Events;

namespace Application.BenefitForms.Commands.CreateBenefitForm
{
    [Authorize(Policy = Policies.BenefitFormManagement)]
    public record CreateBenefitFormCommand : IRequest<BenefitFormDto>
    {
        public string Name { get; init; }
    }

    public class CreateBenefitFormCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateBenefitFormCommand, BenefitFormDto>
    {
        public async Task<BenefitFormDto> Handle(CreateBenefitFormCommand request, CancellationToken cancellationToken)
        {
            var benefitForm = new BenefitForm
            {
                Name = request.Name
            };

            benefitForm.AddDomainEvent(new BenefitFormCreatedEvent(benefitForm));

            await unitOfWork.BenefitFormRepository.InsertAsync(benefitForm, cancellationToken);

            await unitOfWork.SaveAsync(cancellationToken);

            return mapper.Map<BenefitFormDto>(benefitForm);
        }
    }
}



