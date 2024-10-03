using Microsoft.AspNetCore.Identity;

namespace Domain.Authentication.Domain
{
    public class AuthenticationUser : IdentityUser
    {
        public int? CVSUserId { get; set; }

        public required byte[] Salt { get; set; }
    }
}
