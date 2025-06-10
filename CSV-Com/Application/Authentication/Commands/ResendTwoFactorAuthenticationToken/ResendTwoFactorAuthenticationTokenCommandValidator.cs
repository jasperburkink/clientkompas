using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;

namespace Application.Authentication.Commands.ResendTwoFactorAuthenticationToken
{
    public class ResendTwoFactorAuthenticationTokenCommandValidator : AbstractValidator<ResendTwoFactorAuthenticationTokenCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public ResendTwoFactorAuthenticationTokenCommandValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;

            RuleFor(cmd => cmd.UserId).ValidateUserId(_resourceMessageProvider);
            RuleFor(cmd => cmd.TwoFactorPendingToken).ValidateToken(_resourceMessageProvider);
        }
    }
}
