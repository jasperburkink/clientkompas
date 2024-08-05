using Application.Common.Interfaces;
using Domain.CVS.Constants;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class AddressValidationRules
    {
        public const string POSTALCODE_REGEX = "^[1-9][0-9]{3} ?(?!sa|sd|ss|SA|SD|SS)[A-Za-z]{2}$";

        public static IRuleBuilderOptions<T, string> ValidateAddressStreetName<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AddressValidationRules), "StreetNameRequired"))
                .MaximumLength(AddressConstants.StreetnameMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AddressValidationRules), "StreetNameMaxLength", AddressConstants.StreetnameMaxLength));
        }

        public static IRuleBuilderOptions<T, string> ValidateAddressPostalCode<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AddressValidationRules), "PostalCodeRequired"))
                .Matches(POSTALCODE_REGEX)
                .WithMessage((rootObject, postalCode) => resourceMessageProvider.GetMessage(typeof(AddressValidationRules), "PostalCodeInvalid", postalCode))
                .MaximumLength(AddressConstants.PostalcodeMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AddressValidationRules), "PostalCodeMaxLength", AddressConstants.PostalcodeMaxLength));
        }

        public static IRuleBuilderOptions<T, int> ValidateAddressHouseNumber<T>(this IRuleBuilder<T, int> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AddressValidationRules), "HouseNumberRequired"))
                .GreaterThan(0)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AddressValidationRules), "HouseNumberNotNegativeValue"))
                .LessThanOrEqualTo(AddressConstants.HouseNumberMaxValue)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AddressValidationRules), "HouseNumberMaxValue", AddressConstants.HouseNumberMaxValue));
        }

        public static IRuleBuilderOptions<T, string> ValidateAddressHouseNumberAddition<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .MaximumLength(AddressConstants.HouseNumberAdditionMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AddressValidationRules), "HousenumberAddtionMaxLength", AddressConstants.HouseNumberAdditionMaxLength));
        }

        public static IRuleBuilderOptions<T, string> ValidateAddressResidence<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AddressValidationRules), "ResidenceRequired"))
                .MaximumLength(AddressConstants.ResidenceMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(AddressValidationRules), "ResidenceMaxLength", AddressConstants.ResidenceMaxLength));
        }
    }
}
