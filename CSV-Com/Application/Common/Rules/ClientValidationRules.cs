using Application.BenefitForms.Queries.GetBenefitForm;
using Application.Clients.Dtos;
using Application.Common.Interfaces.CVS;
using Application.Diagnoses.Queries.GetDiagnosis;
using Application.DriversLicences.Queries;
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
                    return await unitOfWork.ClientRepository.ExistsAsync(c => c.Id == id, cancellationToken);
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
                .EmailAddress().WithMessage($"{nameof(Client.EmailAddress)} is geen geldig e-mailadres.")
                .MaximumLength(ClientConstants.EmailAddressMaxLength).WithMessage($"{nameof(Client.EmailAddress)}  mag niet langer zijn dan {ClientConstants.EmailAddressMaxLength} karakters.")
                .MustAsync(async (email, cancellationToken) =>
                {
                    return !await unitOfWork.ClientRepository.ExistsAsync(c => c.EmailAddress == email, cancellationToken);
                }).WithMessage($"{nameof(Client.EmailAddress)} is al in gebruik voor een andere cliënt.");
        }

        public static IRuleBuilderOptions<T, DateOnly> ValidateClientDateOfBirth<T>(this IRuleBuilder<T, DateOnly> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"{nameof(Client.DateOfBirth)} is verplicht.")
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now)).WithMessage($"{nameof(Client.DateOfBirth)} kan niet in de toekomst liggen.");
        }

        public static IRuleBuilderOptions<T, string> ValidateClientRemarks<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .MaximumLength(ClientConstants.RemarksMaxLength).WithMessage($"{nameof(Client.Remarks)} mag niet langer zijn dan {ClientConstants.RemarksMaxLength} karakters.");
        }

        public static IRuleBuilderOptions<T, Gender> ValidateClientGender<T>(this IRuleBuilder<T, Gender> ruleBuilder)
        {
            return ruleBuilder
                .IsInEnum().WithMessage($"{nameof(Client.Gender)} heeft een ongeldige waarde.")
                .NotNull().WithMessage($"{nameof(Client.Gender)} is verplicht.");
        }

        public static IRuleBuilderOptions<T, ICollection<EmergencyPersonDto>> ValidateClientEmergencyPeople<T>(this IRuleBuilder<T, ICollection<EmergencyPersonDto>> ruleBuilder)
        {
            return ruleBuilder
                .NotNull().NotEmpty().WithMessage($"Er moet ten minste één {nameof(Client.EmergencyPeople)} zijn opgegeven.");
        }

        public static IRuleBuilderOptions<T, ICollection<DiagnosisDto>> ValidateClientDiagnoses<T>(this IRuleBuilder<T, ICollection<DiagnosisDto>> ruleBuilder)
        {
            return ruleBuilder
                .NotNull().WithMessage($"{nameof(Client.Diagnoses)} mag niet leeg zijn.");
        }

        public static IRuleBuilderOptions<T, ICollection<BenefitFormDto>> ValidateClientBenefitForms<T>(this IRuleBuilder<T, ICollection<BenefitFormDto>> ruleBuilder)
        {
            return ruleBuilder
                .NotNull().WithMessage($"{nameof(Client.BenefitForms)} mag niet leeg zijn.");
        }

        public static IRuleBuilderOptions<T, ICollection<DriversLicenceDto>> ValidateClientDriversLicences<T>(this IRuleBuilder<T, ICollection<DriversLicenceDto>> ruleBuilder)
        {
            return ruleBuilder
                .NotNull().WithMessage($"{nameof(Client.DriversLicences)} mag niet leeg zijn.");
        }

        public static IRuleBuilderOptions<T, ICollection<ClientWorkingContractDto>> ValidateClientWorkingContracts<T>(this IRuleBuilder<T, ICollection<ClientWorkingContractDto>> ruleBuilder)
        {
            return ruleBuilder
                .NotNull().WithMessage($"{nameof(Client.WorkingContracts)} mag niet leeg zijn.");
        }
    }
}
