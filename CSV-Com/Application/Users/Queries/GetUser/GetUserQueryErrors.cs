using Application.Common.Models;

namespace Application.Users.Queries.GetUser
{
    public static class GetUserQueryErrors
    {
        public static readonly Error UserNotFound = new($"{nameof(GetUserQueryErrors)}.{nameof(UserNotFound)}", "Gebruiker niet gevonden.");
    }
}
