using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class Argon2PasswordHasher : IPasswordHasher<AuthenticationUser>
    {
        public string HashPassword(AuthenticationUser user, string password)
        {
            var hasher = new Argon2Hasher();
            var salt = user.Salt ?? hasher.GenerateSalt(); // Unique salt per user
            user.Salt = salt;
            return hasher.HashString(password, salt);
        }

        public PasswordVerificationResult VerifyHashedPassword(AuthenticationUser user, string hashedPassword, string providedPassword)
        {
            var hasher = new Argon2Hasher();

            var hashedProvidedPassword = hasher.HashString(providedPassword, user.Salt);

            return hashedProvidedPassword == hashedPassword ?
                PasswordVerificationResult.Success :
                PasswordVerificationResult.Failed;
        }
    }
}
