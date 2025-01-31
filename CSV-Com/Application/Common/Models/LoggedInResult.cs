using Domain.Authentication.Domain;

namespace Application.Common.Models
{
    public class LoggedInResult(bool succeeded, AuthenticationUser? user = null, IList<string>? roles = null)
    {
        public bool Succeeded { get; set; } = succeeded;

        public AuthenticationUser? User { get; set; } = user;

        public IList<string>? Roles { get; set; } = roles;
    }
}
