using Domain.Authentication.Domain;

namespace Application.Common.Models
{
    public class LoggedInResult
    {
        public bool Succeeded { get; set; }

        public AuthenticationUser? User { get; set; }

        public IList<string>? Roles { get; set; }

        public LoggedInResult(bool succeeded, AuthenticationUser? user = null, IList<string>? roles = null)
        {
            Succeeded = succeeded;
            User = user;
            Roles = roles;
        }
    }
}
