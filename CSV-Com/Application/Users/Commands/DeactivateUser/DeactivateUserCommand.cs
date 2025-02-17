using Application.Common.Interfaces.CVS;
using Application.Common.Models;

namespace Application.Users.Commands.DeactivateUser
{
    public record DeactivateUserCommand : IRequest<Result>
    {
        public int Id { get; init; }
    }

    public class DeactivateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<DeactivateUserCommand, Result>
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

            return Result.Success();
        }
    }
}
