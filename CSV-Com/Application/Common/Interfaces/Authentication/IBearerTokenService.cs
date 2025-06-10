using Domain.Authentication.Domain;

namespace Application.Common.Interfaces.Authentication
{
    public interface IBearerTokenService
    {
        Task<string> GenerateBearerTokenAsync(IAuthenticationUser user, IList<string> roles);
    }
}
