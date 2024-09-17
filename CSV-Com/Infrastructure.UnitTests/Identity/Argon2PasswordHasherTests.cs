using Domain.Authentication.Domain;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.UnitTests.Identity
{
    public class Argon2PasswordHasherTests
    {
        private readonly Argon2PasswordHasher _passwordHasher;

        public Argon2PasswordHasherTests()
        {
            _passwordHasher = new Argon2PasswordHasher();
        }

        [Fact]
        public void HashPassword_ShouldReturnHashedPassword()
        {
            // Arrange
            var user = new AuthenticationUser { Salt = new byte[16] }; // Mocking user with default salt
            var password = "TestPassword123";

            // Act
            var hashedPassword = _passwordHasher.HashPassword(user, password);

            // Assert
            Assert.NotNull(hashedPassword);
            Assert.NotEmpty(hashedPassword);
        }

        [Fact]
        public void VerifyHashedPassword_ShouldReturnSuccess_ForCorrectPassword()
        {
            // Arrange
            var user = new AuthenticationUser
            {
                Salt = new Argon2Hasher().GenerateSalt()
            };
            var password = "TestPassword123";
            var hashedPassword = _passwordHasher.HashPassword(user, password);

            // Act
            var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, password);

            // Assert
            Assert.Equal(PasswordVerificationResult.Success, result);
        }

        [Fact]
        public void VerifyHashedPassword_ShouldReturnFailed_ForIncorrectPassword()
        {
            // Arrange
            var user = new AuthenticationUser
            {
                Salt = new Argon2Hasher().GenerateSalt()
            };
            var correctPassword = "TestPassword123";
            var hashedPassword = _passwordHasher.HashPassword(user, correctPassword);

            var incorrectPassword = "WrongPassword";

            // Act
            var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, incorrectPassword);

            // Assert
            Assert.Equal(PasswordVerificationResult.Failed, result);
        }

        [Fact]
        public void VerifyHashedPassword_ShouldReturnFailed_WhenUserSaltIsNull()
        {
            // Arrange
            var user = new AuthenticationUser
            {
                Salt = null
            };
            var password = "TestPassword123";
            var hashedPassword = _passwordHasher.HashPassword(user, password);

            // Act
            var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, password);

            // Assert
            Assert.Equal(PasswordVerificationResult.Failed, result);
        }
    }
}
