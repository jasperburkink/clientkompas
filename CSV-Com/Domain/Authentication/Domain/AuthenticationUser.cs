using Microsoft.AspNetCore.Identity;

namespace Domain.Authentication.Domain
{
    public class AuthenticationUser : IdentityUser
    {
        public int CVSUserId { get; set; }

        public byte[]? Salt { get; set; }

        public bool HasTemporaryPassword { get; set; }

        public DateTime? TemporaryPasswordExpiryDate { get; set; }

        public int TemporaryPasswordTokenCount { get; set; }
    }
}
