using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;

namespace Application.Authentication.Commands.TwoFactorAuthentication
{
    public class TwoFactorAuthenticationCommandValidator : AbstractValidator<TwoFactorAuthenticationCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public TwoFactorAuthenticationCommandValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;

            RuleFor(cmd => cmd.Token).ValidateToken(_resourceMessageProvider);
            RuleFor(cmd => cmd.UserId).ValidateUserId(_resourceMessageProvider);
        }
    }
}
