using System.Text;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Constants;

namespace Infrastructure.Identity
{
    public class PasswordService : IPasswordService
    {
        private const string LOWERCASE = "abcdefghijklmnopqrstuvwxyz";
        private const string UPPERCASE = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string DIGITS = "1234567890";
        private const string SPECIAL_CHARS = "!@#$%^&*()_-+=<>?";
        private const string ALL_VALID_CHARS = LOWERCASE + UPPERCASE + DIGITS + SPECIAL_CHARS;

        public string GeneratePassword(int length)
        {
            if (length < 4)
            {
                throw new ArgumentException("Password length must be at least 4 to include all required character types.");
            }

            var res = new StringBuilder();
            var rnd = new Random();

            // Password should at least contain one of each type of characters
            res.Append(LOWERCASE[rnd.Next(LOWERCASE.Length)]);
            res.Append(UPPERCASE[rnd.Next(UPPERCASE.Length)]);
            res.Append(DIGITS[rnd.Next(DIGITS.Length)]);
            res.Append(SPECIAL_CHARS[rnd.Next(SPECIAL_CHARS.Length)]);

            // Add chars to get length of password
            for (var i = res.Length; i < length; i++)
            {
                res.Append(ALL_VALID_CHARS[rnd.Next(ALL_VALID_CHARS.Length)]);
            }

            // Shuffle password characters so the first four characters are shuffled in the password
            return Shuffle(res.ToString(), rnd);
        }

        public bool IsValidPassword(string password)
        {

            // Password should be minimal length
            if (password.Length < AuthenticationUserConstants.PASSWORD_MINLENGTH)
            {
                return false;
            }

            // Password should not exceed maximal length
            if (password.Length > AuthenticationUserConstants.PASSWORD_MAXLENGTH)
            {
                return false;
            }

            // Password should contain one digit
            if (!password.Any(DIGITS.Contains))
            {
                return false;
            }

            // Password should contain one lowercase character
            if (!password.Any(LOWERCASE.Contains))
            {
                return false;
            }

            // Password should contain one uppercase character
            if (!password.Any(UPPERCASE.Contains))
            {
                return false;
            }

            // Password should contain one special character
            if (!password.Any(SPECIAL_CHARS.Contains))
            {
                return false;
            }

            return true;
        }

        private string Shuffle(string str, Random rnd)
        {
            var array = str.ToCharArray();

            for (var i = array.Length - 1; i > 0; i--)
            {
                var j = rnd.Next(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }

            return new string(array);
        }
    }
}
