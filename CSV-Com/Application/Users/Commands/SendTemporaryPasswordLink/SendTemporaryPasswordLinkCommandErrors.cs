using Application.Common.Models;

namespace Application.Users.Commands.SendTemporaryPasswordLink
{
    public static class SendTemporaryPasswordLinkCommandErrors
    {
        public static readonly Error UserHasNoTemporaryPassword = new($"{nameof(SendTemporaryPasswordLinkCommandErrors)}.{nameof(UserHasNoTemporaryPassword)}", "Deze gebruiker heeft geen tijdelijk wachtwoord.");

        public static readonly Error UserHasNoEmailAddress = new($"{nameof(SendTemporaryPasswordLinkCommandErrors)}.{nameof(UserHasNoEmailAddress)}", "Geen e-mail adres gevonden voor deze gebruiker.");

        public static readonly Error UserNotFound = new($"{nameof(SendTemporaryPasswordLinkCommandErrors)}.{nameof(UserNotFound)}", "Gebruiker niet gevonden.");

        public static readonly Error CreatedByUserNotFound = new($"{nameof(SendTemporaryPasswordLinkCommandErrors)}.{nameof(CreatedByUserNotFound)}", "Gebruiker die deze gebruiker heeft aangemaakt is niet gevonden.");

        public static readonly Error EmailAddressContactPersonNotFound = new($"{nameof(SendTemporaryPasswordLinkCommandErrors)}.{nameof(CreatedByUserNotFound)}", "Er is geen e-mailadres gevonden voor het contactpersoon.");
    }
}
