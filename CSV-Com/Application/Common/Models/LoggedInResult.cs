using Domain.Authentication.Domain;

namespace Application.Common.Models
{
    public class LoggedInResult(bool succeeded, IAuthenticationUser? user = null, IList<string>? roles = null)
    {
        public bool Succeeded { get; set; } = succeeded;

        public IAuthenticationUser? User { get; set; } = user;

        public IList<string>? Roles { get; set; } = roles;
    }
}
