using Application.Common.Interfaces;
using Domain.CVS.Constants;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class DriversLicenceValidationRules
    {
        public static IRuleBuilderOptions<T, string> ValidateDriversLicenceCategory<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(DriversLicenceValidationRules), "CategoryRequired"))
                .MaximumLength(DriversLicenceConstants.CATEGORY_MAXLENGTH)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(DriversLicenceValidationRules), "CategoryMaxLength", DriversLicenceConstants.CATEGORY_MAXLENGTH));
        }

        public static IRuleBuilderOptions<T, string> ValidateDriversLicenceDescription<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(DriversLicenceValidationRules), "DescriptionRequired"))
                .MaximumLength(DriversLicenceConstants.DESCRIPTION_MAXLENGTH)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(DriversLicenceValidationRules), "DescriptionMaxLength", DriversLicenceConstants.DESCRIPTION_MAXLENGTH));
        }
    }
}
