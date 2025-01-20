using Domain.Authentication.Domain;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class TemporaryPasswordToken : IdentityUserToken<string>, IToken
    {
        public required DateTime ExpiresAt { get; set; }

        public required DateTime CreatedAt { get; set; }

        public required bool IsUsed { get; set; }

        public required bool IsRevoked { get; set; }

        public bool IsExpired => ExpiresAt < DateTime.UtcNow;
    }
}
