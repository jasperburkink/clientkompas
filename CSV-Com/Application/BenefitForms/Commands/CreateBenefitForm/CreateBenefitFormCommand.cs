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

    public class CreateBenefitFormCommandHandler : IRequestHandler<CreateBenefitFormCommand, BenefitFormDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateBenefitFormCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BenefitFormDto> Handle(CreateBenefitFormCommand request, CancellationToken cancellationToken)
        {
            var benefitForm = new BenefitForm
            {
                Name = request.Name
            };

            benefitForm.AddDomainEvent(new BenefitFormCreatedEvent(benefitForm));

            await _unitOfWork.BenefitFormRepository.InsertAsync(benefitForm, cancellationToken);

            await _unitOfWork.SaveAsync(cancellationToken);

            return _mapper.Map<BenefitFormDto>(benefitForm);
        }
    }
}



