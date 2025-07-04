﻿using Application.Common.Interfaces;
using Domain.CVS.Constants;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class EmergencyPersonValidationRules
    {
        public static IRuleBuilderOptions<T, string> ValidateEmergencyPersonName<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(EmergencyPersonValidationRules), "NameRequired"))
                .MaximumLength(EmergencyPersonConstants.NameMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(EmergencyPersonValidationRules), "NameMaxLength", EmergencyPersonConstants.NameMaxLength));
        }

        public static IRuleBuilderOptions<T, string> ValidateEmergencyPersonTelephoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(EmergencyPersonValidationRules), "TelephoneNumberRequired"))
                .MaximumLength(EmergencyPersonConstants.TelephoneNumberMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(EmergencyPersonValidationRules), "TelephoneNumberMaxLength", EmergencyPersonConstants.TelephoneNumberMaxLength));
        }
    }
}
