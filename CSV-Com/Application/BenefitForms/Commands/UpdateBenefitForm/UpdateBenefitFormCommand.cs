using Application.BenefitForms.Queries.GetBenefitForm;
using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;

namespace Application.BenefitForms.Commands.UpdateBenefitForm
{
    [Authorize(Policy = Policies.BenefitFormManagement)]
    public record UpdateBenefitFormCommand : IRequest<BenefitFormDto>
    {
        public int Id { get; init; }

        public string Name { get; set; }
    }

    public class UpdateBenefitFormCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateBenefitFormCommand, BenefitFormDto>
    {
        public async Task<BenefitFormDto> Handle(UpdateBenefitFormCommand request, CancellationToken cancellationToken)
        {

            var benefitForm = await unitOfWork.BenefitFormRepository.GetByIDAsync(request.Id, cancellationToken)
                ?? throw new NotFoundException(nameof(BenefitForm), request.Id);


            benefitForm.Name = request.Name;

            await unitOfWork.SaveAsync(cancellationToken);

            return mapper.Map<BenefitFormDto>(benefitForm);
        }
    }
}
