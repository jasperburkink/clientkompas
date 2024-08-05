using Application.Common.Interfaces;
using Domain.CVS.Constants;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class DiagnosisValidationRules
    {
        public static IRuleBuilderOptions<T, string> ValidateDiagnosisName<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(DiagnosisValidationRules), "NameRequired"))
                .MaximumLength(DiagnosisConstants.NAME_MAXLENGTH)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(DiagnosisValidationRules), "NameMaxLength", DiagnosisConstants.NAME_MAXLENGTH));
        }
    }
}
