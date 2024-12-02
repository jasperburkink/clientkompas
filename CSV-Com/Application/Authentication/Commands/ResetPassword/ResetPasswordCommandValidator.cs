using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;

namespace Application.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IResourceMessageProvider _resourceMessageProvider;

        public ResetPasswordCommandValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            _unitOfWork = unitOfWork;
            _resourceMessageProvider = resourceMessageProvider;
            RuleFor(rpc => rpc.EmailAddress).ValidateEmailAddress(_resourceMessageProvider);
            RuleFor(rpc => rpc.NewPassword).ValidatePassword(_resourceMessageProvider, rpc => rpc.NewPasswordRepeat);
            RuleFor(rpc => rpc.NewPasswordRepeat).ValidatePassword(_resourceMessageProvider, rpc => rpc.NewPasswordRepeat); // Same selector as value, don't want to check same value of password repeat.
            RuleFor(rpc => rpc.Token).ValidateToken(_resourceMessageProvider);
        }
    }
}
