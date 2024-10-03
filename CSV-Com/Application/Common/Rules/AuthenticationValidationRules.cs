using Application.Common.Interfaces;
using Domain.Authentication.Constants;

namespace Application.Common.Rules
{
    public static class AuthenticationValidationRules
    {
        public static IRuleBuilderOptions<T, string> ValidateUserName<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AuthenticationValidationRules), "UserNameRequired"))
                .MaximumLength(AuthenticationUserConstants.USERNAME_MAXLENGTH)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AuthenticationValidationRules), "UserNameMaxLength", AuthenticationUserConstants.USERNAME_MAXLENGTH));
        }

        public static IRuleBuilderOptions<T, string> ValidatePassword<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AuthenticationValidationRules), "PasswordRequired"))
                .MaximumLength(AuthenticationUserConstants.PASSWORD_MAXLENGTH)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AuthenticationValidationRules), "PasswordMaxLength", AuthenticationUserConstants.PASSWORD_MAXLENGTH));
        }
    }
}
