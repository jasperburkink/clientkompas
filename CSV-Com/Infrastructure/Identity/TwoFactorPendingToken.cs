using Domain.Authentication.Domain;

namespace Infrastructure.Identity
{
    public class TwoFactorPendingToken : AuthenticationUserToken, IAuthenticationToken
    {
    }
}
