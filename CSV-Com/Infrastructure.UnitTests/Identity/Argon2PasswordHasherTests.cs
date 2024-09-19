using Domain.Authentication.Domain;
using FluentAssertions;
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
            hashedPassword.Should().NotBeNull().And.NotBeEmpty();
        }

        [Fact]
        public void VerifyHashedPassword_CorrectPassword_ShouldReturnSuccess()
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
            result.Should().Be(PasswordVerificationResult.Success);
        }

        [Fact]
        public void VerifyHashedPassword_IncorrectPassword_ShouldReturnFailed()
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
            result.Should().Be(PasswordVerificationResult.Failed);
        }

        [Fact]
        public void VerifyHashedPassword_UserSaltIsNull_ShouldCreateUserSalt()
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
            user.Salt.Should().NotBeEmpty();
            result.Should().Be(PasswordVerificationResult.Success);
        }
    }
}
