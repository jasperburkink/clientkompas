using Application.Common.Interfaces;
using Application.Common.Validators;
using Domain.CVS.Constants;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class EmergencyPersonRules
    {
        public static IRuleBuilderOptions<T, string> ValidateEmergencyPersonName<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage<EmergencyPersonValidator>("NameRequired"))
                .MaximumLength(EmergencyPersonConstants.NameMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage<EmergencyPersonValidator>("NameMaxLength", EmergencyPersonConstants.NameMaxLength));
        }

        public static IRuleBuilderOptions<T, string> ValidateEmergencyPersonTelephoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage<EmergencyPersonValidator>("TelephoneNumberRequired"))
                .MaximumLength(EmergencyPersonConstants.TelephoneNumberMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage<EmergencyPersonValidator>("TelephoneNumberMaxLength", EmergencyPersonConstants.TelephoneNumberMaxLength));
        }
    }
}
