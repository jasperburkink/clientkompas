using Domain.Authentication.Domain;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AuthenticationUserRole : IdentityUserRole<string>, IAuthenticationUserRole
    {
    }
}
