using Application.Common.Interfaces.CVS;
using Application.Common.Rules;
using Domain.CVS.ValueObjects;
using FluentValidation;

namespace Application.Common.Validators
{
    public class AddressValidator : AbstractValidator<Address>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(a => a.StreetName).ValidateAddressStreetName();
            RuleFor(a => a.HouseNumber).ValidateAddressHouseNumber();
            RuleFor(a => a.HouseNumberAddition).ValidateAddressHouseNumberAddition();
            RuleFor(a => a.PostalCode).ValidateAddressPostalCode();
            RuleFor(a => a.Residence).ValidateAddressResidence();
        }
    }
}
