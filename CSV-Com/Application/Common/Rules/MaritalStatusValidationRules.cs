using Domain.CVS.Constants;
using Domain.CVS.Domain;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class MaritalStatusValidationRules
    {
        public static IRuleBuilderOptions<T, string> ValidateMaritalStatusName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(MaritalStatus.Name)} is verplicht.")
                .MaximumLength(MaritalStatusConstants.NAME_MAXLENGTH).WithMessage($"{nameof(MaritalStatus.Name)} mag niet langer zijn dan {MaritalStatusConstants.NAME_MAXLENGTH} karakters.");
        }
    }
}
