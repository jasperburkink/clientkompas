using Application.Common.Models;

namespace Application.Users.Commands.DeactivateUser
{
    public static class DeactivateUserCommandErrors
    {
        public static readonly Error UserNotFound = new($"{nameof(DeactivateUserCommandErrors)}.{nameof(UserNotFound)}", "Gebruiker met id '{0}' kan niet worden gevonden.");

        public static readonly Error UserAlreadyDeactivated = new($"{nameof(DeactivateUserCommandErrors)}.{nameof(UserAlreadyDeactivated)}", "Gebruiker met id '{0}' is al gedeactiveerd op {1} en kan dus niet opnieuw worden gedeactiveerd.");
    }
}
