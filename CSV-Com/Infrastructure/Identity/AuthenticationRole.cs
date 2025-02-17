using Domain.Authentication.Domain;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AuthenticationRole : IdentityRole, IAuthenticationRole
    {
        public AuthenticationRole()
        {
        }

        public AuthenticationRole(string roleName) : base(roleName)
        {
        }
    }
}
