using Application.Common.Interfaces;

namespace Application.Common.Rules
{
    public static class RefreshTokenValidationRules
    {
        public static IRuleBuilderOptions<T, string?> ValidateRefreshToken<T>(this IRuleBuilder<T, string?> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(RefreshTokenValidationRules), "TokenRequired"));
        }
    }
}
