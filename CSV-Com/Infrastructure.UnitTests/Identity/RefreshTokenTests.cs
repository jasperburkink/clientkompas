using FluentAssertions;
using Infrastructure.Identity;

namespace Infrastructure.UnitTests.Identity
{
    public class RefreshTokenTests
    {
        [Fact]
        public void RefreshToken_InitializeWithProperties_PropertiesAreSet()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var tokenValue = "dit-is-een-token-waarde";
            var expiresAt = DateTime.UtcNow.AddHours(1);
            var createdAt = DateTime.UtcNow;
            var isUsed = false;
            var isRevoked = false;

            var token = new RefreshToken
            {
                UserId = userId,
                Value = tokenValue,
                ExpiresAt = expiresAt,
                CreatedAt = createdAt,
                IsUsed = isUsed,
                IsRevoked = isRevoked
            };

            // Act & Assert
            token.UserId.Should().Be(userId);
            token.Value.Should().Be(tokenValue);
            token.ExpiresAt.Should().Be(expiresAt);
            token.CreatedAt.Should().Be(createdAt);
            token.IsUsed.Should().Be(isUsed);
            token.IsRevoked.Should().Be(isRevoked);
        }

        [Fact]
        public void IsExpired_TokenIsExpired_ShouldBeTrue()
        {
            // Arrange
            var token = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false,
                IsUsed = true,
                ExpiresAt = DateTime.UtcNow.AddMinutes(-1)
            };

            // Act & Assert
            token.IsExpired.Should().BeTrue();
        }
    }
}
