using System.Text;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Domain;
using FluentAssertions;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using TestData;

namespace Infrastructure.FunctionalTests.Identity
{
    public class RefreshTokenServiceTests : IDisposable
    {
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly AuthenticationUser _authenticationUser;
        private readonly AuthenticationDbContext _authenticationDbContext;
        private const string REFRESH_TOKEN_VALUE = nameof(REFRESH_TOKEN_VALUE);

        public RefreshTokenServiceTests()
        {
            var options = new DbContextOptionsBuilder<AuthenticationDbContext>()
                        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                        .Options;

            _authenticationDbContext = new AuthenticationDbContext(options);

            _authenticationUser = new AuthenticationUser
            {
                UserName = FakerConfiguration.Faker.Person.UserName,
                Email = FakerConfiguration.Faker.Person.Email,
                Salt = Encoding.UTF8.GetBytes("Salt"),
                PasswordHash = "Test",
                PhoneNumber = FakerConfiguration.Faker.Person.Phone
            };

            _authenticationDbContext.AuthenticationUsers.Add(_authenticationUser);
            _authenticationDbContext.SaveChanges();

            var hasher = new Argon2Hasher();

            _refreshTokenService = new RefreshTokenService(_authenticationDbContext, hasher);
        }

        [Fact]
        public async Task GenerateRefreshTokenAsync_CorrectFlow_ShouldGenerateValidToken()
        {
            // Arrange
            var user = _authenticationUser;

            // Act
            var token = await _refreshTokenService.GenerateRefreshTokenAsync(user);

            // Assert
            token.Should().NotBeNull();
            token.Should().BeOfType<string>();
        }

        [Fact]
        public async Task GenerateRefreshTokenAsync_AuthenticationUserIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            AuthenticationUser user = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _refreshTokenService.GenerateRefreshTokenAsync(user));
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_CorrectFlow_ShouldReturnTrue()
        {
            // Arrange
            var userId = _authenticationUser.Id;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                Name = "Test",
                UserId = userId,
                LoginProvider = "Test",
                Value = REFRESH_TOKEN_VALUE
            };

            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject);
            await _authenticationDbContext.SaveChangesAsync();

            // Act
            var result = await _refreshTokenService.ValidateRefreshTokenAsync(userId, refreshToken);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_ChangedIsUsedStatusOfToken_TokenIsUsedIsTrue()
        {
            // Arrange
            var userId = _authenticationUser.Id;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                Name = "Test",
                UserId = userId,
                LoginProvider = "Test",
                Value = REFRESH_TOKEN_VALUE
            };

            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject);
            await _authenticationDbContext.SaveChangesAsync();

            // Act
            await _refreshTokenService.ValidateRefreshTokenAsync(userId, refreshToken);

            // Assert
            refreshTokenObject.IsUsed.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_TokenAlreadyUsed_ShouldReturnFalse()
        {
            // Assert
            var userId = _authenticationUser.Id;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = true,
                Name = "Test",
                UserId = userId,
                LoginProvider = "Test",
                Value = REFRESH_TOKEN_VALUE
            };

            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject);
            await _authenticationDbContext.SaveChangesAsync();

            // Act
            var result = await _refreshTokenService.ValidateRefreshTokenAsync(userId, refreshToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_TokenIsRevoked_ShouldReturnFalse()
        {
            // Assert
            var userId = _authenticationUser.Id;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = true,
                IsUsed = false,
                Name = "Test",
                UserId = userId,
                LoginProvider = "Test",
                Value = REFRESH_TOKEN_VALUE
            };

            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject);
            await _authenticationDbContext.SaveChangesAsync();

            // Act
            var result = await _refreshTokenService.ValidateRefreshTokenAsync(userId, refreshToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_NoTokensForThisUser_ShouldReturnFalse()
        {
            // Assert
            var userId = _authenticationUser.Id;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                Name = "Test",
                UserId = "Test",
                LoginProvider = "Test",
                Value = REFRESH_TOKEN_VALUE
            };

            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject);
            await _authenticationDbContext.SaveChangesAsync();

            // Act
            var result = await _refreshTokenService.ValidateRefreshTokenAsync(userId, refreshToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_NoTokensWithSpecificValue_ShouldReturnFalse()
        {
            // Assert
            var userId = _authenticationUser.Id;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                Name = "Test",
                UserId = userId,
                LoginProvider = "Test",
                Value = "Test"
            };

            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject);
            await _authenticationDbContext.SaveChangesAsync();

            // Act
            var result = await _refreshTokenService.ValidateRefreshTokenAsync(userId, refreshToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_TokenExpired_ShouldReturnFalse()
        {
            // Arrange
            var userId = _authenticationUser.Id;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(-1),
                IsRevoked = false,
                IsUsed = false,
                Name = "Test",
                UserId = userId,
                LoginProvider = "Test",
                Value = REFRESH_TOKEN_VALUE
            };

            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject);
            await _authenticationDbContext.SaveChangesAsync();

            // Act
            var result = await _refreshTokenService.ValidateRefreshTokenAsync(userId, refreshToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_UserIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            string userId = null;
            var refreshToken = "RefreshToken";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _refreshTokenService.ValidateRefreshTokenAsync(userId, refreshToken));
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_RefreshTokenIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            string userId = null;
            var refreshToken = "RefreshToken";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _refreshTokenService.ValidateRefreshTokenAsync(userId, refreshToken));
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_CorrectFlow_TokenIsRevoked()
        {
            // Arrange
            var userId = _authenticationUser.Id;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                Name = "Test",
                UserId = userId,
                LoginProvider = "Test",
                Value = REFRESH_TOKEN_VALUE
            };

            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject);
            await _authenticationDbContext.SaveChangesAsync();

            // Act
            await _refreshTokenService.RevokeRefreshTokenAsync(userId, refreshToken);
            var token = await _refreshTokenService.GetRefreshTokenAsync(refreshToken);

            // Assert
            token.Should().NotBeNull();
            token!.IsRevoked.Should().BeTrue();
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_UserIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            string userId = null;
            var refreshToken = "RefreshToken";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _refreshTokenService.RevokeRefreshTokenAsync(userId, refreshToken));
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_RefreshTokenIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var userId = "test";
            string refreshToken = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _refreshTokenService.RevokeRefreshTokenAsync(userId, refreshToken));
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_NoTokensForThisUser_TokenNotRevoked()
        {
            // Assert
            var userId = _authenticationUser.Id;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(-1),
                IsRevoked = false,
                IsUsed = false,
                Name = "Test",
                UserId = "Test",
                LoginProvider = "Test",
                Value = REFRESH_TOKEN_VALUE
            };

            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject);
            await _authenticationDbContext.SaveChangesAsync();

            // Act
            await _refreshTokenService.RevokeRefreshTokenAsync(userId, refreshToken);
            var token = await _refreshTokenService.GetRefreshTokenAsync(refreshToken);

            // Assert
            token.Should().NotBeNull();
            token!.IsRevoked.Should().BeFalse();
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_NoTokensWithSpecificValue_TokenNotRevoked()
        {
            // Arrange
            var userId = _authenticationUser.Id;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                Name = "Test",
                UserId = userId,
                LoginProvider = "Test",
                Value = "Test"
            };

            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject);
            await _authenticationDbContext.SaveChangesAsync();

            // Act
            await _refreshTokenService.RevokeRefreshTokenAsync(userId, refreshToken);
            var token = await _refreshTokenService.GetRefreshTokenAsync("Test");

            // Assert
            token.Should().NotBeNull();
            token!.IsRevoked.Should().BeFalse();
        }

        [Fact]
        public async Task GetRefreshTokenAsync_CorrectFlow_ReturnsRefreshToken()
        {
            // Arrange
            var userId = _authenticationUser.Id;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                Name = "Test",
                UserId = userId,
                LoginProvider = "Test",
                Value = refreshToken
            };

            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject);
            await _authenticationDbContext.SaveChangesAsync();

            // Act
            var result = await _refreshTokenService.GetRefreshTokenAsync(refreshToken);

            // Assert
            result.Should().NotBeNull().And.BeEquivalentTo(refreshTokenObject);
        }

        [Fact]
        public async Task GetRefreshTokenAsync_NotTokensWithSpecificValue_ReturnsNull()
        {
            // Arrange
            var userId = _authenticationUser.Id;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                Name = "Test",
                UserId = userId,
                LoginProvider = "Test",
                Value = "Test"
            };

            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject);
            await _authenticationDbContext.SaveChangesAsync();

            // Act
            var result = await _refreshTokenService.GetRefreshTokenAsync(refreshToken);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetValidRefreshTokensByUserAsync_CorrectFlowMultipleTokens_ReturnsMultipleValidRefreshToken()
        {
            // Arrange
            var userId = _authenticationUser.Id;
            var refreshToken1 = REFRESH_TOKEN_VALUE;
            var refreshToken2 = "AlsoARefreshToken";

            var refreshTokenObject1 = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                Name = "Test1",
                UserId = userId,
                LoginProvider = "Test",
                Value = refreshToken1
            };

            var refreshTokenObject2 = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                Name = "Test2",
                UserId = userId,
                LoginProvider = "Test",
                Value = refreshToken2
            };

            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject1);
            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject2);
            await _authenticationDbContext.SaveChangesAsync();

            // Act
            var result = await _refreshTokenService.GetValidRefreshTokensByUserAsync(userId);

            // Assert
            result.Should().NotBeNull().And.BeEquivalentTo(new List<RefreshToken> { refreshTokenObject1, refreshTokenObject2 });
        }

        [Fact]
        public async Task GetValidRefreshTokensByUserAsync_NoValidTokens_ReturnsEmptyList()
        {
            // Arrange
            var userId = _authenticationUser.Id;
            var refreshToken1 = REFRESH_TOKEN_VALUE;
            var refreshToken2 = "AlsoARefreshToken";

            var refreshTokenObject1 = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = true,
                IsUsed = false,
                Name = "Test1",
                UserId = userId,
                LoginProvider = "Test",
                Value = refreshToken1
            };

            var refreshTokenObject2 = new RefreshToken
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = true,
                Name = "Test2",
                UserId = userId,
                LoginProvider = "Test",
                Value = refreshToken2
            };

            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject1);
            await _authenticationDbContext.UserTokens.AddAsync(refreshTokenObject2);
            await _authenticationDbContext.SaveChangesAsync();

            // Act
            var result = await _refreshTokenService.GetValidRefreshTokensByUserAsync(userId);

            // Assert
            result.Should().NotBeNull().And.BeEmpty();
        }

        public void Dispose()
        {
            _authenticationDbContext.Database.EnsureDeleted();
            _authenticationDbContext.Dispose();
        }
    }
}
