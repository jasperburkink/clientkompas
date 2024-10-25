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
                .MinimumLength(AuthenticationUserConstants.PASSWORD_MINLENGTH)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AuthenticationValidationRules), "PasswordMinLength", AuthenticationUserConstants.PASSWORD_MINLENGTH))
                .MaximumLength(AuthenticationUserConstants.PASSWORD_MAXLENGTH)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AuthenticationValidationRules), "PasswordMaxLength", AuthenticationUserConstants.PASSWORD_MAXLENGTH))
                .Matches(@"[a-z]")
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AuthenticationValidationRules), "PasswordContainsLowerCaseCharacter"))
                .Matches(@"[A-Z]")
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AuthenticationValidationRules), "PasswordContainsUpperCaseCharacter"))
                .Matches(@"[0-9]")
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AuthenticationValidationRules), "PasswordContainsNumber"))
                .Matches(@"[\W]")
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AuthenticationValidationRules), "PasswordContainsSpecialCharacter"));
        }
    }
}
