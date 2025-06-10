using Application.Clients.Dtos;
using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;
using FluentValidation;

namespace Application.Common.Validators
{
    public class EmergencyPersonValidator : AbstractValidator<EmergencyPersonDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public EmergencyPersonValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;
            RuleFor(ep => ep.Name).ValidateEmergencyPersonName(resourceMessageProvider);
            RuleFor(ep => ep.TelephoneNumber).ValidateEmergencyPersonTelephoneNumber(resourceMessageProvider);
        }
    }
}
