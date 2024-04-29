using Domain.Constants;
using Domain.CVS.Domain;
using FluentValidation;

namespace Application.Clients.Rules
{
    public static class ClientValidationRules
    {
        public static IRuleBuilderOptions<T, string> ValidateFirstName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Client.FirstName)} is verplicht.")
                .MaximumLength(ClientConstants.ClientFirstNameMaxLength).WithMessage($"{nameof(Client.FirstName)} mag niet langer zijn dan {ClientConstants.ClientFirstNameMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateLastName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Client.LastName)} is verplicht.")
                .MaximumLength(ClientConstants.ClientLastNameMaxLength).WithMessage($"{nameof(Client.LastName)}  mag niet langer zijn dan {ClientConstants.ClientLastNameMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidatePrefixLastName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(ClientConstants.ClientPrefixLastNameMaxLength).WithMessage($"{nameof(Client.PrefixLastName)}  mag niet langer zijn dan {ClientConstants.ClientPrefixLastNameMaxLength} karakters.");
        }
    }
}
