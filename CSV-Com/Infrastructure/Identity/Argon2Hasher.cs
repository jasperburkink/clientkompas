using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace Infrastructure.Identity
{
    public class Argon2Hasher
    {
        private const int NUM_OF_THREADS = 8;
        private const int NUM_OF_ITERATIONS = 4;
        private const int MEMORYMB = 1024 * 64; // MEMORY 64 MB
        private const int HASH_BYTES = 32;
        private const int SALT_BYTES = 16;

        public string HashPassword(string password, byte[] salt)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            var argon2 = new Argon2id(passwordBytes)
            {
                Salt = salt,
                DegreeOfParallelism = NUM_OF_THREADS,
                Iterations = NUM_OF_ITERATIONS,
                MemorySize = MEMORYMB
            };

            var hashBytes = argon2.GetBytes(HASH_BYTES);

            return Convert.ToBase64String(hashBytes);
        }

        public byte[] GenerateSalt(int size = SALT_BYTES)
        {
            var salt = new byte[size];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }
    }
}
