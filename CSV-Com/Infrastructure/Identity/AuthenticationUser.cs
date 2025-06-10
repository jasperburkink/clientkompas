using Domain.Authentication.Domain;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AuthenticationUser : IdentityUser, IAuthenticationUser
    {
        public int CVSUserId { get; set; }

        public byte[]? Salt { get; set; }

        public bool HasTemporaryPassword { get; set; }

        public DateTime? TemporaryPasswordExpiryDate { get; set; }

        public int TemporaryPasswordTokenCount { get; set; }
    }
}
