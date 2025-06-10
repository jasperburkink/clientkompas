using Application.Common.Interfaces;
using Domain.CVS.Constants;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class BenefitFormValidationRules
    {
        public static IRuleBuilderOptions<T, string> ValidateBenefitFormName<T>(this IRuleBuilder<T, string> ruleBuilder,
             IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(BenefitFormValidationRules), "NameRequired"))
                .MaximumLength(BenefitFormConstants.NAME_MAXLENGTH)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(BenefitFormValidationRules), "NameMaxLength", BenefitFormConstants.NAME_MAXLENGTH));
        }
    }
}
