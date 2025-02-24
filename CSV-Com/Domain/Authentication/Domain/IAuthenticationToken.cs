namespace Domain.Authentication.Domain
{
    public interface IAuthenticationToken
    {
        string UserId { get; set; }

        string Value { get; set; }

        DateTime ExpiresAt { get; set; }

        DateTime CreatedAt { get; set; }

        bool IsUsed { get; set; }

        bool IsRevoked { get; set; }

        bool IsExpired { get; }
    }
}
