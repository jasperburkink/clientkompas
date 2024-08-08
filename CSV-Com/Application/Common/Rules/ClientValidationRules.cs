using Application.BenefitForms.Queries.GetBenefitForm;
using Application.Clients.Dtos;
using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Diagnoses.Queries.GetDiagnosis;
using Application.DriversLicences.Queries;
using Domain.CVS.Constants;
using Domain.CVS.Enums;
using FluentValidation;

namespace Application.Common.Rules
{
    public static class ClientValidationRules
    {
        public static IRuleBuilderOptions<T, int> ValidateClientExists<T>(this IRuleBuilder<T, int> ruleBuilder, IUnitOfWork unitOfWork,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .MustAsync(async (id, cancellationToken) =>
                {
                    return await unitOfWork.ClientRepository.ExistsAsync(c => c.Id == id, cancellationToken);
                })
                .WithMessage(id => resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "ClientDoesNotExists", id));
        }

        public static IRuleBuilderOptions<T, string> ValidateClientFirstName<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "FirstNameRequired"))
                .MaximumLength(ClientConstants.FirstNameMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "FirstNameMaxLength", ClientConstants.FirstNameMaxLength));
        }

        public static IRuleBuilderOptions<T, string> ValidateClientLastName<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "LastNameRequired"))
                .MaximumLength(ClientConstants.LastNameMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "LastNameMaxLength", ClientConstants.LastNameMaxLength));
        }

        public static IRuleBuilderOptions<T, string> ValidateClientPrefixLastName<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .MaximumLength(ClientConstants.PrefixLastNameMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "TussenvoegselMaxLength", ClientConstants.PrefixLastNameMaxLength));
        }

        public static IRuleBuilderOptions<T, string> ValidateClientInitials<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "InitialsRequired"))
                .MaximumLength(ClientConstants.InitialsMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "InitialsMaxLength", ClientConstants.InitialsMaxLength));
        }

        public static IRuleBuilderOptions<T, string> ValidateClientTelephoneNumber<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "TelephoneNumberRequired"))
                .MaximumLength(ClientConstants.TelephoneNumberMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "TelephoneNumberMaxLength", ClientConstants.TelephoneNumberMaxLength));
        }

        public static IRuleBuilderOptions<T, bool> ValidateClientIsInTargetGroupRegister<T>(this IRuleBuilder<T, bool> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotNull().WithMessage($"{nameof(Client.IsInTargetGroupRegister)} is verplicht.");
        }

        public static IRuleBuilderOptions<T, string> ValidateClientEmailAddress<T>(this IRuleBuilder<T, string> ruleBuilder, IUnitOfWork unitOfWork, Func<T, int> getCurrentClientId,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "EmailAddressRequired"))
                .EmailAddress()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "EmailAddressInvalid"))
                .MaximumLength(ClientConstants.EmailAddressMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "EmailAddressMaxLength", ClientConstants.EmailAddressMaxLength))
                .MustAsync(async (client, email, cancellationToken) =>
                {
                    var currentClientId = getCurrentClientId(client);
                    return !await unitOfWork.ClientRepository.ExistsAsync(c => c.EmailAddress == email && c.Id != currentClientId, cancellationToken);
                })
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "EmailAddresInUse"));
        }

        public static IRuleBuilderOptions<T, DateOnly> ValidateClientDateOfBirth<T>(this IRuleBuilder<T, DateOnly> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "DateOfBirthRequired"))
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "DateOfBirthNotInFuture"));
        }

        public static IRuleBuilderOptions<T, string> ValidateClientRemarks<T>(this IRuleBuilder<T, string> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .MaximumLength(ClientConstants.RemarksMaxLength)
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "RemarksMaxLength", ClientConstants.RemarksMaxLength));
        }

        public static IRuleBuilderOptions<T, Gender> ValidateClientGender<T>(this IRuleBuilder<T, Gender> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .IsInEnum()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "GenderRequired"))
                .NotNull()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "GenderInvalid"));
        }

        public static IRuleBuilderOptions<T, ICollection<EmergencyPersonDto>> ValidateClientEmergencyPeople<T>(this IRuleBuilder<T, ICollection<EmergencyPersonDto>> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotNull().NotEmpty()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "EmergencyPeopleRequired"));
        }

        public static IRuleBuilderOptions<T, ICollection<DiagnosisDto>> ValidateClientDiagnoses<T>(this IRuleBuilder<T, ICollection<DiagnosisDto>> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotNull()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "DiagnosisEmpty"));
        }

        public static IRuleBuilderOptions<T, ICollection<BenefitFormDto>> ValidateClientBenefitForms<T>(this IRuleBuilder<T, ICollection<BenefitFormDto>> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotNull()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "BenefitFormsEmpty"));
        }

        public static IRuleBuilderOptions<T, ICollection<DriversLicenceDto>> ValidateClientDriversLicences<T>(this IRuleBuilder<T, ICollection<DriversLicenceDto>> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotNull()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "DriversLicencesEmpty"));
        }

        public static IRuleBuilderOptions<T, ICollection<ClientWorkingContractDto>> ValidateClientWorkingContracts<T>(this IRuleBuilder<T, ICollection<ClientWorkingContractDto>> ruleBuilder,
            IResourceMessageProvider resourceMessageProvider)
        {
            return ruleBuilder
                .NotNull()
                .WithMessage(resourceMessageProvider.GetMessage(typeof(ClientValidationRules), "WorkingContractsEmpty"));
        }
    }
}
