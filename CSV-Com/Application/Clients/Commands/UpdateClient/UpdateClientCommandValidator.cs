using Application.Clients.Rules;
using Application.Common.Interfaces.CVS;
using FluentValidation;

namespace Application.Clients.Commands.UpdateClient
{
    internal class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateClientCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(c => c.FirstName).ValidateFirstName();
            RuleFor(c => c.LastName).ValidateLastName();
            RuleFor(c => c.PrefixLastName).ValidatePrefixLastName();
        }
    }
}
