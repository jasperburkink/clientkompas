using Domain.Authentication.Domain;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class Argon2PasswordHasher : IPasswordHasher<AuthenticationUser>
    {
        public string HashPassword(AuthenticationUser user, string password)
        {
            var hasher = new Argon2Hasher();
            var salt = hasher.GenerateSalt(); // Unique salt per user
            return hasher.HashPassword(password, salt);
        }

        public PasswordVerificationResult VerifyHashedPassword(AuthenticationUser user, string hashedPassword, string providedPassword)
        {
            var hasher = new Argon2Hasher();

            var hashedProvidedPassword = hasher.HashPassword(providedPassword, user.Salt);

            if (hashedProvidedPassword == hashedPassword)
            {
                return PasswordVerificationResult.Success;
            }
            else
            {
                return PasswordVerificationResult.Failed;
            }
        }
    }
}
