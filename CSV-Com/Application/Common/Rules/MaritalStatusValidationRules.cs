using Application.Common.Interfaces;
using Domain.CVS.Constants;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class MaritalStatusValidationRules
    {
        public static IRuleBuilderOptions<T, string> ValidateMaritalStatusName<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(MaritalStatusValidationRules), "NameRequired"))
                .MaximumLength(MaritalStatusConstants.NAME_MAXLENGTH)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(MaritalStatusValidationRules), "NameMaxLength", MaritalStatusConstants.NAME_MAXLENGTH));
        }
    }
}
