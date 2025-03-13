namespace Application.Common.Models.Authentication
{
    public static class IdentityServiceErrors
    {
        public static readonly Error UserEmailAddressNotFound = new($"{nameof(IdentityServiceErrors)}.{nameof(UserEmailAddressNotFound)}", "Er is geen gebruiker gevonden met het gegeven e-mailadres.");

        public static readonly Error UserNotFound = new($"{nameof(IdentityServiceErrors)}.{nameof(UserNotFound)}", "Authentication gebruiker met id '{0}' niet gevonden.");

        public static readonly Error RoleNotFound = new($"{nameof(IdentityServiceErrors)}.{nameof(RoleNotFound)}", "Rol '{0}' niet gevonden.");


    }
}
