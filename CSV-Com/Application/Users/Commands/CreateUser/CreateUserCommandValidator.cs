using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Rules;

namespace Application.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(IUnitOfWork unitOfWork, IResourceMessageProvider resourceMessageProvider)
        {
            RuleFor(u => u.FirstName).ValidateUserFirstName(resourceMessageProvider);
            RuleFor(u => u.PrefixLastName).ValidateUserPrefixLastName(resourceMessageProvider);
            RuleFor(u => u.LastName).ValidateUserLastName(resourceMessageProvider);
            RuleFor(u => u.EmailAddress).ValidateUserEmailAddress(unitOfWork, resourceMessageProvider);
            RuleFor(u => u.TelephoneNumber).ValidateUserTelephoneNumber(unitOfWork, resourceMessageProvider);
            RuleFor(u => u.RoleName).ValidateUserRole(resourceMessageProvider);
        }
    }
}
