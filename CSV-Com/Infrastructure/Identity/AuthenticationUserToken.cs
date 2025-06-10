using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AuthenticationUserToken : IdentityUserToken<string>
    {
        public required DateTime ExpiresAt { get; set; }

        public required DateTime CreatedAt { get; set; }

        public required bool IsUsed { get; set; }

        public required bool IsRevoked { get; set; }

        public bool IsExpired => ExpiresAt < DateTime.UtcNow;
    }
}
