using System.Text;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using FluentAssertions;
using Infrastructure.Identity;

namespace Infrastructure.UnitTests.Identity
{
    public class BearerTokenServiceTests
    {
        [Fact]
        public async Task GenerateBearerTokenAsync_CorrectFlow_ShouldGenerateValidToken()
        {
            // Arrange
            var user = new AuthenticationUser
            {
                Id = "123",
                UserName = "testuser",
                Email = "test@example.com",
                CVSUserId = 1,
                Salt = Encoding.UTF8.GetBytes("Salt")
            };
            var roles = new List<string> { nameof(Roles.Administrator), nameof(Roles.Coach) };
            var tokenService = new BearerTokenService();

            // Act
            var token = await tokenService.GenerateBearerTokenAsync(user, roles);

            // Assert
            token.Should().NotBeNull();
            token.Should().BeOfType<string>();
        }

        [Fact]
        public async Task GenerateBearerTokenAsync_AuthenticationUserIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            AuthenticationUser user = null;
            var roles = new List<string> { nameof(Roles.Administrator) };
            var tokenService = new BearerTokenService();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => tokenService.GenerateBearerTokenAsync(user, roles));
        }

        [Fact]
        public async Task GenerateBearerTokenAsync_RolesIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var user = new AuthenticationUser
            {
                Id = "123",
                UserName = "testuser",
                Email = "test@example.com",
                CVSUserId = 1,
                Salt = Encoding.UTF8.GetBytes("Salt")
            };
            IList<string> roles = null;
            var tokenService = new BearerTokenService();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => tokenService.GenerateBearerTokenAsync(user, roles));
        }
    }
}
