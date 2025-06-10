using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;
using Domain.CVS.ValueObjects;
using FluentValidation;

namespace Application.Common.Validators
{
    public class AddressValidator : AbstractValidator<Address>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public AddressValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;
            RuleFor(a => a.StreetName).ValidateAddressStreetName(_resourceMessageProvider);
            RuleFor(a => a.HouseNumber).ValidateAddressHouseNumber(_resourceMessageProvider);
            RuleFor(a => a.HouseNumberAddition).ValidateAddressHouseNumberAddition(_resourceMessageProvider);
            RuleFor(a => a.PostalCode).ValidateAddressPostalCode(_resourceMessageProvider);
            RuleFor(a => a.Residence).ValidateAddressResidence(_resourceMessageProvider);
        }
    }
}
