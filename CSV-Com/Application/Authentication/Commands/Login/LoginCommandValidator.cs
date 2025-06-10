using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;

namespace Application.Authentication.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public LoginCommandValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;

            RuleFor(lc => lc.UserName).ValidateUserName(_resourceMessageProvider);
            RuleFor(lc => lc.Password).ValidatePasswordLogin(_resourceMessageProvider);
        }
    }
}
