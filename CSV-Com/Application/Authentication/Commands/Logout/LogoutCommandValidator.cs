using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;

namespace Application.Authentication.Commands.Logout
{
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public LogoutCommandValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;
            RuleFor(rtc => rtc.RefreshToken).ValidateRefreshToken(_resourceMessageProvider);
        }
    }
}
