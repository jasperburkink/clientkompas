using Domain.CVS.Constants;
using Domain.CVS.Domain;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class DiagnosisValidationRules
    {
        public static IRuleBuilderOptions<T, string> ValidateDiagnosisName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Diagnosis.Name)} is verplicht.")
                .MaximumLength(DiagnosisConstants.NAME_MAXLENGTH).WithMessage($"{nameof(Diagnosis.Name)} mag niet langer zijn dan {DiagnosisConstants.NAME_MAXLENGTH} karakters.");
        }
    }
}
