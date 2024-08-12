using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;
using Application.Common.Validators;
using FluentValidation;

namespace Application.Clients.Commands.CreateClient
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public CreateClientCommandValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;
            RuleFor(c => c.FirstName).ValidateClientFirstName(_resourceMessageProvider);
            RuleFor(c => c.LastName).ValidateClientLastName(_resourceMessageProvider);
            RuleFor(c => c.PrefixLastName).ValidateClientPrefixLastName(_resourceMessageProvider);
            RuleFor(c => c.Initials).ValidateClientInitials(_resourceMessageProvider);
            RuleFor(c => c.StreetName).ValidateAddressStreetName(_resourceMessageProvider);
            RuleFor(c => c.PostalCode).ValidateAddressPostalCode(_resourceMessageProvider);
            RuleFor(c => c.HouseNumber).ValidateAddressHouseNumber(_resourceMessageProvider);
            RuleFor(c => c.HouseNumberAddition).ValidateAddressHouseNumberAddition(_resourceMessageProvider);
            RuleFor(c => c.Residence).ValidateAddressResidence(_resourceMessageProvider);
            RuleFor(c => c.TelephoneNumber).ValidateClientTelephoneNumber(_resourceMessageProvider);
            RuleFor(c => c.EmailAddress).ValidateClientEmailAddress(_unitOfWork, client => 0, _resourceMessageProvider);
            RuleFor(c => c.IsInTargetGroupRegister).ValidateClientIsInTargetGroupRegister(_resourceMessageProvider);
            RuleFor(c => c.DateOfBirth).ValidateClientDateOfBirth(_resourceMessageProvider);
            RuleFor(c => c.Remarks).ValidateClientRemarks(_resourceMessageProvider);
            RuleFor(c => c.EmergencyPeople).ValidateClientEmergencyPeople(_resourceMessageProvider);
            RuleForEach(c => c.EmergencyPeople).SetValidator(new EmergencyPersonValidator(_unitOfWork, _resourceMessageProvider));
            RuleFor(c => c.Gender).ValidateClientGender(_resourceMessageProvider);
            RuleFor(c => c.Diagnoses).ValidateClientDiagnoses(_resourceMessageProvider);
            RuleFor(c => c.BenefitForms).ValidateClientBenefitForms(_resourceMessageProvider);
            RuleFor(c => c.DriversLicences).ValidateClientDriversLicences(_resourceMessageProvider);
            RuleFor(c => c.WorkingContracts).ValidateClientWorkingContracts(_resourceMessageProvider);
            RuleForEach(c => c.WorkingContracts).SetValidator(new WorkingContractValidator(_unitOfWork, _resourceMessageProvider));
        }
    }
}
