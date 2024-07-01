using Domain.CVS.Constants;
using Domain.CVS.Domain;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class EmergencyPersonRules
    {
        public static IRuleBuilderOptions<T, string> ValidateEmergencyPersonName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(EmergencyPerson.Name)} is verplicht.")
                .MaximumLength(EmergencyPersonConstants.NameMaxLength).WithMessage($"{nameof(EmergencyPerson.Name)} mag niet langer zijn dan {EmergencyPersonConstants.NameMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateEmergencyPersonTelephoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(EmergencyPerson.TelephoneNumber)} is verplicht.")
                .MaximumLength(EmergencyPersonConstants.TelephoneNumberMaxLength).WithMessage($"{nameof(EmergencyPerson.TelephoneNumber)} mag niet langer zijn dan {EmergencyPersonConstants.TelephoneNumberMaxLength} karakters.");
        }
    }
}
