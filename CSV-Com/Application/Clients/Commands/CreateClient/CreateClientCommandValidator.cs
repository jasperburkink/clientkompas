using Application.Common.Interfaces.CVS;
using Application.Common.Rules;
using Application.Common.Validators;
using FluentValidation;

namespace Application.Clients.Commands.CreateClient
{
    public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateClientCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(c => c.FirstName).ValidateClientFirstName();
            RuleFor(c => c.LastName).ValidateClientLastName();
            RuleFor(c => c.PrefixLastName).ValidateClientPrefixLastName();
            RuleFor(c => c.Initials).ValidateClientInitials();
            RuleFor(c => c.StreetName).ValidateClientStreetName();
            RuleFor(c => c.PostalCode).ValidateClientPostalCode();
            RuleFor(c => c.HouseNumber).ValidateClientHouseNumber();
            RuleFor(c => c.HouseNumberAddition).ValidateClientHouseNumberAddition();
            RuleFor(c => c.Residence).ValidateClientResidence();
            RuleFor(c => c.TelephoneNumber).ValidateClientTelephoneNumber();
            RuleFor(c => c.EmailAddress).ValidateClientEmailAddress(_unitOfWork);
            RuleFor(c => c.DateOfBirth).ValidateClientDateOfBirth();
            RuleFor(c => c.Remarks).ValidateClientRemarks();
            RuleFor(c => c.EmergencyPeople).ValidateClientEmergencyPeople();
            RuleFor(c => c.Gender).ValidateClientGender();
            RuleForEach(c => c.EmergencyPeople).SetValidator(new EmergencyPersonValidator(_unitOfWork));
            RuleForEach(c => c.WorkingContracts).SetValidator(new WorkingContractValidator(_unitOfWork));
        }
    }
}
