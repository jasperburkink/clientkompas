using Domain.CVS.Constants;
using Domain.CVS.Domain;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class BenefitFormValidationRules
    {
        public static IRuleBuilderOptions<T, string> ValidateDiagnosisName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(BenefitForm.Name)} is verplicht.")
                .MaximumLength(BenefitFormConstants.NAME_MAXLENGTH).WithMessage($"{nameof(BenefitForm.Name)} mag niet langer zijn dan {BenefitFormConstants.NAME_MAXLENGTH} karakters.");
        }
    }
}
