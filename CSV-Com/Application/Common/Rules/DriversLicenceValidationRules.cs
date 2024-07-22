using Domain.CVS.Constants;
using Domain.CVS.Domain;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class DriversLicenceValidationRules
    {
        public static IRuleBuilderOptions<T, string> ValidateDriversLicenceCategory<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(DriversLicence.Category)} is verplicht.")
                .MaximumLength(DriversLicenceConstants.CATEGORY_MAXLENGTH).WithMessage($"{nameof(DriversLicence.Category)} mag niet langer zijn dan {DriversLicenceConstants.CATEGORY_MAXLENGTH} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateDriversLicenceDescription<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(DriversLicence.Description)} is verplicht.")
                .MaximumLength(DriversLicenceConstants.DESCRIPTION_MAXLENGTH).WithMessage($"{nameof(DriversLicence.Category)} mag niet langer zijn dan {DriversLicenceConstants.DESCRIPTION_MAXLENGTH} karakters.");
        }
    }
}
