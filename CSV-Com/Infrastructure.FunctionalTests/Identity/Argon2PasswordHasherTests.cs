using Domain.Authentication.Domain;
using FluentAssertions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.FunctionalTests.Identity
{
    public class Argon2PasswordHasherTests
    {
        private readonly Argon2PasswordHasher _passwordHasher;

        public Argon2PasswordHasherTests()
        {
            _passwordHasher = new Argon2PasswordHasher();
        }

        [Fact]
        public void HashPassword_ValidUserAndPassword_ReturnsHashedPasswordAndSetsSalt()
        {
            // Arrange
            var user = new AuthenticationUser { Salt = null! };
            var password = "ValidPassword123";

            // Act
            var hashedPassword = _passwordHasher.HashPassword(user, password);

            // Assert
            hashedPassword.Should().NotBeNullOrEmpty();
            user.Salt.Should().NotBeNull();
            user.Salt.Length.Should().Be(16);
        }

        [Fact]
        public void VerifyHashedPassword_CorrectPassword_ReturnsSuccess()
        {
            // Arrange
            var user = new AuthenticationUser { Salt = new byte[16] };
            var password = "CorrectPassword123";
            var hashedPassword = _passwordHasher.HashPassword(user, password);

            // Act
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, hashedPassword, password);

            // Assert
            verificationResult.Should().Be(PasswordVerificationResult.Success);
        }

        [Fact]
        public void VerifyHashedPassword_IncorrectPassword_ReturnsFailed()
        {
            // Arrange
            var user = new AuthenticationUser { Salt = new byte[16] };
            var correctPassword = "CorrectPassword123";
            var incorrectPassword = "IncorrectPassword123";
            var hashedPassword = _passwordHasher.HashPassword(user, correctPassword);

            // Act
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, hashedPassword, incorrectPassword);

            // Assert
            verificationResult.Should().Be(PasswordVerificationResult.Failed);
        }

        [Fact]
        public void HashPassword_SamePasswordDifferentUsers_ReturnsDifferentHashes()
        {
            // Arrange
            var user1 = new AuthenticationUser { Salt = null! };
            var user2 = new AuthenticationUser { Salt = null! };
            var password = "SamePassword123";

            // Act
            var hashedPassword1 = _passwordHasher.HashPassword(user1, password);
            var hashedPassword2 = _passwordHasher.HashPassword(user2, password);

            // Assert
            hashedPassword1.Should().NotBe(hashedPassword2);
        }

        [Fact]
        public void HashPassword_NewSaltGeneratedWhenNoneProvided_SetsSaltOnUser()
        {
            // Arrange
            var user = new AuthenticationUser { Salt = null! };
            var password = "PasswordWithoutSalt";

            // Act
            var hashedPassword = _passwordHasher.HashPassword(user, password);

            // Assert
            hashedPassword.Should().NotBeNullOrEmpty();
            user.Salt.Should().NotBeNull();
        }

        [Fact]
        public void VerifyHashedPassword_EmptyHashedPassword_ReturnsFailed()
        {
            // Arrange
            var user = new AuthenticationUser { Salt = new byte[16] };
            var providedPassword = "Password123";

            // Act
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, "", providedPassword);

            // Assert
            verificationResult.Should().Be(PasswordVerificationResult.Failed);
        }

        [Fact]
        public void VerifyHashedPassword_ValidHashedPasswordWithGeneratedSalt_ReturnsSuccess()
        {
            // Arrange
            var user = new AuthenticationUser { Salt = null! };
            var password = "PasswordWithGeneratedSalt";
            var hashedPassword = _passwordHasher.HashPassword(user, password);

            // Act
            var verificationResult = _passwordHasher.VerifyHashedPassword(user, hashedPassword, password);

            // Assert
            verificationResult.Should().Be(PasswordVerificationResult.Success);
            user.Salt.Should().NotBeNull();
        }

        [Fact]
        public void HashPassword_SamePasswordDifferentUsersDifferentSalts_ReturnsDifferentHashes()
        {
            // Arrange
            var user1 = new AuthenticationUser { Salt = new Argon2Hasher().GenerateSalt() };
            var user2 = new AuthenticationUser { Salt = new Argon2Hasher().GenerateSalt() };
            var password = "SamePassword123";

            // Act
            var hashedPassword1 = _passwordHasher.HashPassword(user1, password);
            var hashedPassword2 = _passwordHasher.HashPassword(user2, password);

            // Assert
            hashedPassword1.Should().NotBe(hashedPassword2);
            user1.Salt.Should().NotBeEquivalentTo(user2.Salt);
        }
    }
}
