using Infrastructure.Identity;

namespace Infrastructure.UnitTests.Identity
{
    public class Argon2HasherTests
    {
        private readonly Argon2Hasher _hasher;

        public Argon2HasherTests()
        {
            _hasher = new Argon2Hasher();
        }

        [Fact]
        public void HashPassword_ShouldReturnBase64String()
        {
            // Arrange
            var password = "TestPassword123";
            var salt = _hasher.GenerateSalt();
            var hashedPasswordLength = 44; // Base64 encoded 32 bytes hash will be 44 characters long

            // Act
            var hashedPassword = _hasher.HashString(password, salt);

            // Assert
            Assert.NotNull(hashedPassword);
            Assert.NotEmpty(hashedPassword);
            Assert.Equal(hashedPasswordLength, hashedPassword.Length);
        }

        [Fact]
        public void GenerateSalt_ShouldReturnBytesOfExpectedLength()
        {
            // Arrange
            var saltLength = 16; // Default salt size

            // Act
            var salt = _hasher.GenerateSalt();

            // Assert
            Assert.NotNull(salt);
            Assert.Equal(saltLength, salt.Length);
        }

        [Fact]
        public void GenerateSalt_WithCustomSize_ShouldReturnBytesOfCustomLength()
        {
            // Arrange
            var customSize = 32;

            // Act
            var salt = _hasher.GenerateSalt(customSize);

            // Assert
            Assert.NotNull(salt);
            Assert.Equal(customSize, salt.Length);
        }

        [Fact]
        public void HashPassword_ShouldNotBeSameForDifferentSalts()
        {
            // Arrange
            var password = "TestPassword123";
            var salt1 = _hasher.GenerateSalt();
            var salt2 = _hasher.GenerateSalt();

            // Act
            var hashedPassword1 = _hasher.HashString(password, salt1);
            var hashedPassword2 = _hasher.HashString(password, salt2);

            // Assert
            Assert.NotEqual(hashedPassword1, hashedPassword2);
        }

        [Fact]
        public void HashPassword_ShouldReturnSameHashForSameInput()
        {
            // Arrange
            var password = "TestPassword123";
            var salt = _hasher.GenerateSalt();

            // Act
            var hashedPassword1 = _hasher.HashString(password, salt);
            var hashedPassword2 = _hasher.HashString(password, salt);

            // Assert
            Assert.Equal(hashedPassword1, hashedPassword2);
        }
    }
}
