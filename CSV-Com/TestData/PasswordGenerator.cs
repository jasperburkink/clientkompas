using System.Security.Cryptography;

namespace TestData
{
    public static class PasswordGenerator
    {
        private const int MINIMAL_PASSWORD_LENGTH = 8;

        public static string GenerateSecurePassword(int length = MINIMAL_PASSWORD_LENGTH)
        {
            if (length < MINIMAL_PASSWORD_LENGTH)
            {
                throw new ArgumentException($"Password length must be at least {MINIMAL_PASSWORD_LENGTH}.");
            }

            const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+";
            const string SpecialChars = "!@#$%^&*()_+";

            var password = new char[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                // Add at least one special character at a random position
                password[randomBytes[0] % length] = SpecialChars[randomBytes[1] % SpecialChars.Length];

                // Fill the rest of the password
                for (var i = 0; i < password.Length; i++)
                {
                    // Skip the position where a special character is already placed
                    if (password[i] == '\0') // '\0' means it's not filled yet
                    {
                        password[i] = ValidChars[randomBytes[i] % ValidChars.Length];
                    }
                }
            }

            return new string(password);
        }
    }
}
