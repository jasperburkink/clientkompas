using Application.BenefitForms.Queries.GetBenefitForm;
using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;
using FluentValidation;

namespace Application.Common.Validators
{
    public class BenefitFormValidator : AbstractValidator<BenefitFormDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public BenefitFormValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;

            RuleFor(bf => bf.Name).ValidateBenefitFormName(_resourceMessageProvider);
        }
    }
}
