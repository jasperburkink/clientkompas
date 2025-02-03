using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;

namespace Application.Users.Commands.SendTemporaryPasswordLinkCommand
{
    public class SendTemporaryPasswordLinkCommandValidator : AbstractValidator<SendTemporaryPasswordLinkCommand>
    {
        public SendTemporaryPasswordLinkCommandValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            RuleFor(dto => dto.UserId).ValidateUserId(resourceMessageProvider);
        }
    }
}
