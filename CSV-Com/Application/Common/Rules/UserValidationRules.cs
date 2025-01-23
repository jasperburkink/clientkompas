using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Constants;

namespace Application.Common.Rules
{
    public static class UserValidationRules
    {
        public static IRuleBuilderOptions<T, string?> ValidateUserFirstName<T>(this IRuleBuilder<T, string?> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(UserValidationRules), "FirstNameRequired"))
                .MaximumLength(UserConstants.EmailAddressMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(UserValidationRules), "FirstNameMaxLength", UserConstants.EmailAddressMaxLength));
        }

        public static IRuleBuilderOptions<T, string?> ValidateUserPrefixLastName<T>(this IRuleBuilder<T, string?> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .MaximumLength(UserConstants.EmailAddressMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(UserValidationRules), "PrefixLastnameMaxLength", UserConstants.EmailAddressMaxLength));
        }

        public static IRuleBuilderOptions<T, string?> ValidateUserLastName<T>(this IRuleBuilder<T, string?> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(UserValidationRules), "LastNameRequired"))
                .MaximumLength(UserConstants.EmailAddressMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(UserValidationRules), "LastNameMaxLength", UserConstants.EmailAddressMaxLength));
        }

        public static IRuleBuilderOptions<T, string?> ValidateUserEmailAddress<T>(this IRuleBuilder<T, string?> ruleBuilder, IUnitOfWork unitOfWork,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(UserValidationRules), "EmailAddressRequired"))
                .EmailAddress()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(UserValidationRules), "EmailAddressInvalid"))
                .MaximumLength(UserConstants.EmailAddressMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(UserValidationRules), "EmailAddressMaxLength", UserConstants.EmailAddressMaxLength))
                .MustAsync(async (emailAddress, cancellationToken) =>
                {
                    return !await unitOfWork.UserRepository.ExistsAsync(u => u.EmailAddress.ToUpper() == emailAddress.ToUpper(), cancellationToken);
                })
                .WithMessage(createCoachingProgramCommandT =>
                {
                    return resourceMessageProvider.GetMessage(
                        typeof(UserValidationRules),
                        "EmailAddressAlreadyExists"
                    );
                });
        }

        public static IRuleBuilderOptions<T, string?> ValidateUserTelephoneNumber<T>(this IRuleBuilder<T, string?> ruleBuilder,
            IUnitOfWork unitOfWork,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(UserValidationRules), "TelephoneNumberRequired"))
                .MaximumLength(UserConstants.TelephoneNumberMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(UserValidationRules), "TelephoneNumberMaxLength", UserConstants.TelephoneNumberMaxLength))
                .MustAsync(async (telephoneNumber, cancellationToken) =>
                {
                    return !await unitOfWork.UserRepository.ExistsAsync(u => u.TelephoneNumber.ToUpper() == telephoneNumber.ToUpper(), cancellationToken);
                })
                .WithMessage(createCoachingProgramCommandT =>
                {
                    return resourceMessageProvider.GetMessage(
                        typeof(UserValidationRules),
                        "TelephoneNumberAlreadyExists"
                    );
                });
        }

        public static IRuleBuilderOptions<T, string?> ValidateUserRole<T>(this IRuleBuilder<T, string?> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(UserValidationRules), "RoleRequired"));
        }
    }
}
