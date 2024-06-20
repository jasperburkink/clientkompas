using Application.Clients.Dtos;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;
using FluentValidation;

namespace Application.Common.Validators
{
    public class EmergencyPersonValidator : AbstractValidator<EmergencyPersonDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmergencyPersonValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(ep => ep.Name).ValidateEmergencyPersonName();
            RuleFor(ep => ep.TelephoneNumber).ValidateEmergencyPersonTelephoneNumber();
        }
    }
}
