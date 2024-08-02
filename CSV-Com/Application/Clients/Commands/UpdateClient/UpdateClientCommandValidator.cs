using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;
using Application.Common.Validators;
using FluentValidation;

namespace Application.Clients.Commands.UpdateClient
{
    public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public UpdateClientCommandValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;
            RuleFor(c => c.FirstName).ValidateClientFirstName();
            RuleFor(c => c.LastName).ValidateClientLastName();
            RuleFor(c => c.PrefixLastName).ValidateClientPrefixLastName();
            RuleFor(c => c.Initials).ValidateClientInitials();
            RuleFor(c => c.StreetName).ValidateAddressStreetName();
            RuleFor(c => c.PostalCode).ValidateAddressPostalCode();
            RuleFor(c => c.HouseNumber).ValidateAddressHouseNumber();
            RuleFor(c => c.HouseNumberAddition).ValidateAddressHouseNumberAddition();
            RuleFor(c => c.Residence).ValidateAddressResidence();
            RuleFor(c => c.TelephoneNumber).ValidateClientTelephoneNumber();
            RuleFor(c => c.EmailAddress).ValidateClientEmailAddress(_unitOfWork, client => client.Id);
            RuleFor(c => c.DateOfBirth).ValidateClientDateOfBirth();
            RuleFor(c => c.Remarks).ValidateClientRemarks();
            RuleFor(c => c.EmergencyPeople).ValidateClientEmergencyPeople();
            RuleForEach(c => c.EmergencyPeople).SetValidator(new EmergencyPersonValidator(_unitOfWork));
            RuleFor(c => c.Gender).ValidateClientGender();
            RuleFor(c => c.Diagnoses).ValidateClientDiagnoses();
            RuleFor(c => c.BenefitForms).ValidateClientBenefitForms();
            RuleFor(c => c.DriversLicences).ValidateClientDriversLicences();
            RuleFor(c => c.WorkingContracts).ValidateClientWorkingContracts();
            RuleForEach(c => c.WorkingContracts).SetValidator(new WorkingContractValidator(_unitOfWork, _resourceMessageProvider));
        }
    }
}
