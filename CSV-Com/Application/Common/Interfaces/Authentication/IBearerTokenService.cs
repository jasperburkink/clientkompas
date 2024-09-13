using Domain.Authentication.Domain;

namespace Application.Common.Interfaces.Authentication
{
    public interface IBearerTokenService
    {
        Task<string> GenerateBearerTokenAsync(AuthenticationUser user, IList<string> roles);
    }
}
