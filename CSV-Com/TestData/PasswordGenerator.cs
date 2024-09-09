using System.Security.Cryptography;

namespace TestData
{
    public static class PasswordGenerator
    {
        public static string GenerateSecurePassword(int length)
        {
            const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+";
            var password = new char[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                var randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                for (var i = 0; i < password.Length; i++)
                {
                    password[i] = ValidChars[randomBytes[i] % ValidChars.Length];
                }
            }

            return new string(password);
        }
    }
}
