namespace Application.Common.Interfaces.Authentication
{
    public interface IBearerTokenService
    {
        Task<string> GenerateBearerTokenAsync();
    }
}
