using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using FluentAssertions;
using Infrastructure.Identity;

namespace Infrastructure.FunctionalTests.Identity
{
    public class BearerTokenServiceTests
    {
        [Fact]
        public async Task GenerateBearerTokenAsync_GenerateToken_GeneratedToken_ShouldBeValid()
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
            var roles = new List<string> { "Admin" };
            var tokenService = new BearerTokenService();

            // Act
            var token = await tokenService.GenerateBearerTokenAsync(user, roles);

            // Assert token
            Assert.NotNull(token);

            // Token validation
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            // Assert claims
            jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.Id);
            jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Name && c.Value == user.UserName);
            jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
            jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Exp && !string.IsNullOrEmpty(c.Value));
            jwtToken.Claims.Should().Contain(c => c.Type == BearerTokenService.CLAIM_NAME_CVSUSERID && c.Value == user.CVSUserId.ToString());
            foreach (var role in roles)
            {
                jwtToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == role);
            }
        }

        [Fact]
        public void GetTokenValidationParameters_CorrectFlow_ShouldReturnValidParameters()
        {
            // Act
            var validationParameters = BearerTokenService.GetTokenValidationParameters();

            // Assert
            validationParameters.Should().NotBeNull();
            validationParameters.ValidateIssuer.Should().BeTrue();
            validationParameters.ValidateAudience.Should().BeTrue();
            validationParameters.ValidateLifetime.Should().BeTrue();
            validationParameters.ValidateIssuerSigningKey.Should().BeTrue();
            validationParameters.ValidIssuer.Should().Be(BearerTokenConstants.ISSUER);
            validationParameters.ValidAudience.Should().Be(BearerTokenConstants.AUDIENCE);
        }
    }
}
