using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Models;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Users.Commands.DeactivateUser
{
    [Authorize(Policy = Policies.UserManagement)]
    public record DeactivateUserCommand : IRequest<Result>
    {
        public int Id { get; init; }
    }

    public class DeactivateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService) : IRequestHandler<DeactivateUserCommand, Result>
    {
        public async Task<Result> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByIDAsync(request.Id, cancellationToken);

            if (user == null)
            {
                return Result.Failure($"Gebruiker met id '${request.Id}' kan niet worden gevonden."); // TODO: Result error object
            }

            if (user.DeactivationDateTime != null)
            {
                return Result.Failure($"Gebruiker met id '${user.Id}' is al gedeactiveerd op ${user.DeactivationDateTime} en kan dus niet opnieuw worden gedeactiveerd."); // TODO: Result error object
            }

            user.Deactivate(DateTime.UtcNow);

            await unitOfWork.UserRepository.UpdateAsync(user);
            await unitOfWork.SaveAsync(cancellationToken);

            // TODO: Rebuild to new emailservice
            emailService.SendEmailAsync(
                user.EmailAddress,
                "Uw account is gedeactiveerd",
                """
                Uw account is gedeactiveerd. 
                U kunt niet langer inloggen in het systeem. 
                Wanneer u opnieuw toegang wilt tot het systeem, verzoeken wij u om contact op te nemen met uw contactpersoon.
                """);

            // TODO: add unit tests for sending email

            return Result.Success();
        }
    }
}
