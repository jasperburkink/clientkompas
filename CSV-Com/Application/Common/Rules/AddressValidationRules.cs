using Application.Common.Interfaces;
using Application.Common.Validators;
using Domain.CVS.Constants;
using Domain.CVS.ValueObjects;
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
                .WithMessage(resourceMessageProvider.GetMessage<AddressValidator>("StreetNameRequired"))
                .MaximumLength(AddressConstants.StreetnameMaxLength).WithMessage($"{nameof(Address.StreetName)}  mag niet langer zijn dan {AddressConstants.StreetnameMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateAddressPostalCode<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Address.PostalCode)} is verplicht.")
                .Matches(POSTALCODE_REGEX).WithMessage((rootObject, postalCode) => $"{nameof(Address.PostalCode)} '{postalCode}' is geen geldige postcode.")
                .MaximumLength(AddressConstants.PostalcodeMaxLength).WithMessage($"{nameof(Address.PostalCode)}  mag niet langer zijn dan {AddressConstants.PostalcodeMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, int> ValidateAddressHouseNumber<T>(this IRuleBuilder<T, int> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Address.HouseNumber)} is verplicht.")
                .GreaterThan(0).WithMessage($"{nameof(Address.HouseNumber)} mag geen negatieve waarde bevatten.")
                .LessThanOrEqualTo(AddressConstants.HouseNumberMaxValue).WithMessage($"{nameof(Address.HouseNumber)} mag niet groter zijn dan {AddressConstants.HouseNumberMaxValue}.");
        }

        public static IRuleBuilderOptions<T, string> ValidateAddressHouseNumberAddition<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .MaximumLength(AddressConstants.HouseNumberAdditionMaxLength).WithMessage($"{nameof(Address.HouseNumberAddition)}  mag niet langer zijn dan {AddressConstants.HouseNumberAdditionMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateAddressResidence<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Address.Residence)} is verplicht.")
                .MaximumLength(AddressConstants.ResidenceMaxLength).WithMessage($"{nameof(Address.Residence)}  mag niet langer zijn dan {AddressConstants.ResidenceMaxLength} karakters.");
        }
    }
}
