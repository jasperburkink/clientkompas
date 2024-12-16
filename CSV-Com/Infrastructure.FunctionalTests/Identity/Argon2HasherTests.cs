using FluentAssertions;
using Infrastructure.Identity;

namespace Infrastructure.FunctionalTests.Identity
{
    public class Argon2HasherTests
    {
        private readonly Argon2Hasher _argon2Hasher;

        public Argon2HasherTests()
        {
            _argon2Hasher = new Argon2Hasher();
        }

        [Fact]
        public void GenerateSalt_DefaultSize_ReturnsSaltWithCorrectLength()
        {
            // Arrange
            var defaultSize = 16;

            // Act
            var salt = _argon2Hasher.GenerateSalt();

            // Assert
            salt.Should().NotBeNull();
            salt.Length.Should().Be(defaultSize);
        }

        [Fact]
        public void GenerateSalt_CustomSize_ReturnsSaltWithSpecifiedLength()
        {
            // Arrange
            var customSize = 32;

            // Act
            var salt = _argon2Hasher.GenerateSalt(customSize);

            // Assert
            salt.Should().NotBeNull();
            salt.Length.Should().Be(customSize);
        }

        [Fact]
        public void HashPassword_ValidPasswordAndSalt_ReturnsHashedPassword()
        {
            // Arrange
            var password = "ValidPassword123";
            var salt = _argon2Hasher.GenerateSalt();

            // Act
            var hashedPassword = _argon2Hasher.HashString(password, salt);

            // Assert
            hashedPassword.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void HashPassword_SamePasswordSameSalt_ReturnsSameHash()
        {
            // Arrange
            var password = "SamePassword123";
            var salt = _argon2Hasher.GenerateSalt();

            // Act
            var hash1 = _argon2Hasher.HashString(password, salt);
            var hash2 = _argon2Hasher.HashString(password, salt);

            // Assert
            hash1.Should().Be(hash2);
        }

        [Fact]
        public void HashPassword_SamePasswordDifferentSalt_ReturnsDifferentHash()
        {
            // Arrange
            var password = "SamePassword123";
            var salt1 = _argon2Hasher.GenerateSalt();
            var salt2 = _argon2Hasher.GenerateSalt();

            // Act
            var hash1 = _argon2Hasher.HashString(password, salt1);
            var hash2 = _argon2Hasher.HashString(password, salt2);

            // Assert
            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void HashPassword_DifferentPasswordSameSalt_ReturnsDifferentHash()
        {
            // Arrange
            var salt = _argon2Hasher.GenerateSalt();
            var password1 = "Password123";
            var password2 = "DifferentPassword123";

            // Act
            var hash1 = _argon2Hasher.HashString(password1, salt);
            var hash2 = _argon2Hasher.HashString(password2, salt);

            // Assert
            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void GenerateSalt_GeneratedSaltIsRandom_ReturnsDifferentSaltEachTime()
        {
            // Act
            var salt1 = _argon2Hasher.GenerateSalt();
            var salt2 = _argon2Hasher.GenerateSalt();

            // Assert
            salt1.Should().NotBeEquivalentTo(salt2);
        }

        [Fact]
        public void HashPassword_ValidPassword_HashHasCorrectLength()
        {
            // Arrange
            var password = "ValidPassword123";
            var salt = _argon2Hasher.GenerateSalt();
            var hashLength = 32;

            // Act
            var hash = _argon2Hasher.HashString(password, salt);
            var hashBytes = Convert.FromBase64String(hash);

            // Assert
            hashBytes.Length.Should().Be(hashLength);
        }
    }
}
