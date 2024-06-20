using Application.Clients.Dtos;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Constants;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class ClientValidationRules
    {
        public static IRuleBuilderOptions<T, int> ValidateClientExists<T>(this IRuleBuilder<T, int> ruleBuilder, IUnitOfWork unitOfWork)
        {
            return ruleBuilder
                .MustAsync(async (id, cancellationToken) =>
                {
                    return !await unitOfWork.ClientRepository.ExistsAsync(c => c.Id == id, cancellationToken);
                }).WithMessage(id => $"Cliënt met {nameof(Client.Id)}:'{id}' bestaat niet.");
        }

        public static IRuleBuilderOptions<T, string> ValidateClientFirstName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Client.FirstName)} is verplicht.")
                .MaximumLength(ClientConstants.FirstNameMaxLength).WithMessage($"{nameof(Client.FirstName)} mag niet langer zijn dan {ClientConstants.FirstNameMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateClientLastName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Client.LastName)} is verplicht.")
                .MaximumLength(ClientConstants.LastNameMaxLength).WithMessage($"{nameof(Client.LastName)}  mag niet langer zijn dan {ClientConstants.LastNameMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateClientPrefixLastName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(ClientConstants.PrefixLastNameMaxLength).WithMessage($"{nameof(Client.PrefixLastName)}  mag niet langer zijn dan {ClientConstants.PrefixLastNameMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateClientInitials<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Client.Initials)} is verplicht.")
                .MaximumLength(ClientConstants.InitialsMaxLength).WithMessage($"{nameof(Client.Initials)}  mag niet langer zijn dan {ClientConstants.InitialsMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateClientStreetName<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Client.Address.StreetName)} is verplicht.")
                .MaximumLength(ClientConstants.StreetnameMaxLength).WithMessage($"{nameof(Client.Address.StreetName)}  mag niet langer zijn dan {ClientConstants.StreetnameMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateClientPostalCode<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Client.Address.PostalCode)} is verplicht.")
                .MaximumLength(ClientConstants.PostalcodeMaxLength).WithMessage($"{nameof(Client.Address.PostalCode)}  mag niet langer zijn dan {ClientConstants.PostalcodeMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, int> ValidateClientHouseNumber<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Client.Address.HouseNumber)} is verplicht.")
                .LessThanOrEqualTo(ClientConstants.HouseNumberMaxLength).WithMessage($"{nameof(Client.Address.HouseNumber)} mag niet groter zijn dan {ClientConstants.HouseNumberMaxLength}.");
        }

        public static IRuleBuilderOptions<T, string> ValidateClientHouseNumberAddition<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(ClientConstants.HouseNumberAdditionMaxLength).WithMessage($"{nameof(Client.Address.HouseNumberAddition)}  mag niet langer zijn dan {ClientConstants.HouseNumberAdditionMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateClientResidence<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Client.Address.Residence)} is verplicht.")
                .MaximumLength(ClientConstants.ResidenceMaxLength).WithMessage($"{nameof(Client.Address.Residence)}  mag niet langer zijn dan {ClientConstants.ResidenceMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateClientTelephoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Client.TelephoneNumber)} is verplicht.")
                .MaximumLength(ClientConstants.TelephoneNumberMaxLength).WithMessage($"{nameof(Client.TelephoneNumber)}  mag niet langer zijn dan {ClientConstants.TelephoneNumberMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, string> ValidateClientEmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder, IUnitOfWork unitOfWork)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Client.EmailAddress)} is verplicht.")
                .MaximumLength(ClientConstants.EmailAddressMaxLength).WithMessage($"{nameof(Client.EmailAddress)}  mag niet langer zijn dan {ClientConstants.EmailAddressMaxLength} karakters.")
                .MustAsync(async (email, cancellationToken) =>
                {
                    return await unitOfWork.ClientRepository.ExistsAsync(c => c.EmailAddress == email, cancellationToken);
                }).WithMessage($"{nameof(Client.EmailAddress)} is al in gebruik voor een andere cliënt.");
        }

        public static IRuleBuilderOptions<T, DateOnly> ValidateClientDateOfBirth<T>(this IRuleBuilder<T, DateOnly> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Client.DateOfBirth)} is verplicht.");
        }

        public static IRuleBuilderOptions<T, string> ValidateClientRemarks<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(ClientConstants.RemarksMaxLength).WithMessage($"{nameof(Client.Remarks)} mag niet langer zijn dan {ClientConstants.RemarksMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, Gender> ValidateClientGender<T>(this IRuleBuilder<T, Gender> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Client.Gender)} is verplicht.");
        }

        public static IRuleBuilderOptions<T, ICollection<EmergencyPersonDto>> ValidateClientEmergencyPeople<T>(this IRuleBuilder<T, ICollection<EmergencyPersonDto>> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"Er moet ten minste één {nameof(Client.EmergencyPeople)} zijn opgegeven.");
        }
    }
}
