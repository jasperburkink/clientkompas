using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using FluentValidation;

namespace Application.Organizations.Rules
{
    public static class OrganizationValidationRules
    {
        public static IRuleBuilderOptions<T, string> ValidateOrganizationName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Organization.OrganizationName)} is verplicht.")
                .MaximumLength(OrganizationConstants.OrganizationNameMaxLength).WithMessage($"{nameof(Organization.OrganizationName)} mag niet langer zijn dan {OrganizationConstants.OrganizationNameMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateContactPersonName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Organization.ContactPersonName)} is verplicht.")
                .MaximumLength(OrganizationConstants.ContactPersonNameMaxLength).WithMessage($"{nameof(Organization.ContactPersonName)} mag niet langer zijn dan {OrganizationConstants.ContactPersonNameMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateContactPersonFunction<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Organization.ContactPersonFunction)} is verplicht.")
                .MaximumLength(OrganizationConstants.ContactPersonFunctionMaxLength).WithMessage($"{nameof(Organization.ContactPersonFunction)} mag niet langer zijn dan {OrganizationConstants.ContactPersonFunctionMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateContactPersonTelephoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Organization.ContactPersonTelephoneNumber)} is verplicht.")
                .MaximumLength(OrganizationConstants.ContactPersonTelephoneNumberMaxLength).WithMessage($"{nameof(Organization.ContactPersonTelephoneNumber)} mag niet langer zijn dan {OrganizationConstants.ContactPersonTelephoneNumberMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateContactPersonMobilephoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Organization.ContactPersonMobilephoneNumber)} is verplicht.")
                .MaximumLength(OrganizationConstants.ContactPersonMobilephoneNumberMaxLength).WithMessage($"{nameof(Organization.ContactPersonMobilephoneNumber)} mag niet langer zijn dan {OrganizationConstants.ContactPersonMobilephoneNumberMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateContactPersonEmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Organization.ContactPersonEmailAddress)} is verplicht.")
                .MaximumLength(OrganizationConstants.ContactPersonEmailAddressMaxLength).WithMessage($"{nameof(Organization.ContactPersonEmailAddress)} mag niet langer zijn dan {OrganizationConstants.ContactPersonEmailAddressMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidatePhoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Organization.PhoneNumber)} is verplicht.")
                .MaximumLength(OrganizationConstants.PhoneNumberMaxLength).WithMessage($"{nameof(Organization.PhoneNumber)} mag niet langer zijn dan {OrganizationConstants.PhoneNumberMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateWebsite<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Organization.Website)} is verplicht.")
                .MaximumLength(OrganizationConstants.WebsiteMaxLength).WithMessage($"{nameof(Organization.Website)} mag niet langer zijn dan {OrganizationConstants.WebsiteMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateEmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Organization.EmailAddress)} is verplicht.")
                .MaximumLength(OrganizationConstants.EmailAddressMaxLength).WithMessage($"{nameof(Organization.EmailAddress)} mag niet langer zijn dan {OrganizationConstants.EmailAddressMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateKVKNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Organization.KVKNumber)} is verplicht.")
                .MaximumLength(OrganizationConstants.KVKNumberMaxLength).WithMessage($"{nameof(Organization.KVKNumber)} mag niet langer zijn dan {OrganizationConstants.KVKNumberMaxLength} karakters.");
            //TODO: Add check for unique KVK value
        }

        public static IRuleBuilderOptions<T, string> ValidateBTWNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Organization.BTWNumber)} is verplicht.")
                .MaximumLength(OrganizationConstants.BTWNumberMaxLength).WithMessage($"{nameof(Organization.BTWNumber)} mag niet langer zijn dan {OrganizationConstants.BTWNumberMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateIBANNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Organization.IBANNumber)} is verplicht.")
                .MaximumLength(OrganizationConstants.IBANNumberMaxLength).WithMessage($"{nameof(Organization.IBANNumber)} mag niet langer zijn dan {OrganizationConstants.IBANNumberMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateBIC<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Organization.BIC)} is verplicht.")
                .MaximumLength(OrganizationConstants.BICMaxLength).WithMessage($"{nameof(Organization.BIC)} mag niet langer zijn dan {OrganizationConstants.BICMaxLength} karakters.");
        }
    }
}
