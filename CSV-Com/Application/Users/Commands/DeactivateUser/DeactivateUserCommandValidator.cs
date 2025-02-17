using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;

namespace Application.Users.Commands.DeactivateUser
{
    public class DeactivateUserCommandValidator : AbstractValidator<DeactivateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public DeactivateUserCommandValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;
            RuleFor(c => c.Id).ValidateUserExists(_unitOfWork, _resourceMessageProvider);
        }
    }
}
