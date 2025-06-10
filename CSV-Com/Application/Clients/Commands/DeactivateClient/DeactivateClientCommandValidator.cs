using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;
using FluentValidation;

namespace Application.Clients.Commands.DeactivateClient
{
    public class DeactivateClientCommandValidator : AbstractValidator<DeactivateClientCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public DeactivateClientCommandValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;
            RuleFor(c => c.Id).ValidateClientExists(unitOfWork, _resourceMessageProvider);
        }
    }
}
