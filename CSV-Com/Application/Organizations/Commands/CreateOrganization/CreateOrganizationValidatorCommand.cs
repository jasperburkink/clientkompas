using Application.Common.Interfaces.CVS;
using Application.Organizations.Rules;
using FluentValidation;

namespace Application.Organizations.Commands.CreateOrganization
{
    public class CreateOrganizationValidatorCommand : AbstractValidator<CreateOrganizationCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrganizationValidatorCommand(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(c => c.OrganizationName).ValidateOrganizationName();
            RuleFor(c => c.ContactPersonName).ValidateContactPersonName();
            RuleFor(c => c.ContactPersonFunction).ValidateContactPersonFunction();
            RuleFor(c => c.ContactPersonTelephoneNumber).ValidateContactPersonTelephoneNumber();
            RuleFor(c => c.ContactPersonMobilephoneNumber).ValidateContactPersonMobilephoneNumber();
            RuleFor(c => c.ContactPersonEmailAddress).ValidateContactPersonEmailAddress();
            RuleFor(c => c.PhoneNumber).ValidatePhoneNumber();
            RuleFor(c => c.Website).ValidateWebsite();
            RuleFor(c => c.EmailAddress).ValidateEmailAddress();
            RuleFor(c => c.KVKNumber).ValidateKVKNumber();
            RuleFor(c => c.BTWNumber).ValidateBTWNumber();
            RuleFor(c => c.IBANNumber).ValidateIBANNumber();
            RuleFor(c => c.BIC).ValidateBIC();
        }
    }
}
