using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;

namespace Application.Authentication.Commands.RequestResetPassword
{
    public class RequestResetPasswordCommandValidator : AbstractValidator<RequestResetPasswordCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public RequestResetPasswordCommandValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;

            RuleFor(rtc => rtc.EmailAddress).ValidateEmailAddress(_resourceMessageProvider);
        }
    }
}
