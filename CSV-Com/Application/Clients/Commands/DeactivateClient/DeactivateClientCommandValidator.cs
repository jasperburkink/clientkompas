using Application.Common.Interfaces.CVS;
using Application.Common.Rules;
using FluentValidation;

namespace Application.Clients.Commands.DeactivateClient
{
    public class DeactivateClientCommandValidator : AbstractValidator<DeactivateClientCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateClientCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(c => c.Id).ValidateClientExists(unitOfWork);
        }
    }
}
