using System.Text;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Domain;
using FluentAssertions;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
using Moq;
using Moq.EntityFrameworkCore;

namespace Infrastructure.UnitTests.Identity
{
    public class RefreshTokenServiceTests
    {
        private readonly RefreshTokenService _tokenService;
        private readonly byte[] _salt = Encoding.UTF8.GetBytes("Salt");
        private readonly string _hashedString;
        private const string USER_ID = nameof(USER_ID);
        private const string REFRESH_TOKEN_VALUE = nameof(REFRESH_TOKEN_VALUE);

        public RefreshTokenServiceTests()
        {
            var refreshTokens = new List<RefreshToken>
            {
                new()
                {
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(1),
                    IsRevoked = false,
                    IsUsed = false,
                    UserId = USER_ID,
                    Value = REFRESH_TOKEN_VALUE

                }
            };

            _hashedString = "TestHash";

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens).ReturnsDbSet(refreshTokens);
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()));
            authenticationDBContextMock.Setup(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()));

            var hasherMock = new Mock<IHasher>();
            hasherMock.Setup(mock => mock.GenerateSalt(It.IsAny<int>())).Returns(_salt);
            hasherMock.Setup(mock => mock.HashString(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(_hashedString);

            _tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);
        }

        [Fact]
        public async Task GenerateRefreshTokenAsync_CorrectFlow_ShouldGenerateValidToken()
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

            // Act
            var token = await _tokenService.GenerateRefreshTokenAsync(user);

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
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tokenService.GenerateRefreshTokenAsync(user));
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_CorrectFlow_ShouldReturnTrue()
        {
            // Arrange
            var userId = USER_ID;
            var refreshToken = REFRESH_TOKEN_VALUE;

            // Act
            var result = await _tokenService.ValidateRefreshTokenAsync(userId, refreshToken);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_ChangedIsUsedStatusOfToken_TokenIsUsedIsTrue()
        {
            // Arrange
            var userId = USER_ID;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken()
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                UserId = USER_ID,
                Value = REFRESH_TOKEN_VALUE
            };

            var refreshTokens = new List<RefreshToken>
            {
                refreshTokenObject
            };

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens).ReturnsDbSet(refreshTokens);
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()));
            authenticationDBContextMock.Setup(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()));

            var hasherMock = new Mock<IHasher>();
            hasherMock.Setup(mock => mock.GenerateSalt(It.IsAny<int>())).Returns(_salt);
            hasherMock.Setup(mock => mock.HashString(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(_hashedString);

            var tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);

            // Act
            var result = await tokenService.ValidateRefreshTokenAsync(userId, refreshToken);

            // Assert
            refreshTokenObject.IsUsed.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_TokenAlreadyUsed_ShouldReturnFalse()
        {
            // Arrange
            var userId = USER_ID;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokens = new List<RefreshToken>
            {
                new()
                {
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(1),
                    IsRevoked = false,
                    IsUsed = true,
                    UserId = USER_ID,
                    Value = REFRESH_TOKEN_VALUE
                }
            };

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens).ReturnsDbSet(refreshTokens);
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()));
            authenticationDBContextMock.Setup(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()));

            var hasherMock = new Mock<IHasher>();
            hasherMock.Setup(mock => mock.GenerateSalt(It.IsAny<int>())).Returns(_salt);
            hasherMock.Setup(mock => mock.HashString(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(_hashedString);

            var tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);

            // Act
            var result = await tokenService.ValidateRefreshTokenAsync(userId, refreshToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_TokenIsRevoked_ShouldReturnFalse()
        {
            // Arrange
            var userId = USER_ID;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokens = new List<RefreshToken>
            {
                new()
                {
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(1),
                    IsRevoked = true,
                    IsUsed = false,
                    UserId = USER_ID,
                    Value = REFRESH_TOKEN_VALUE
                }
            };

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens).ReturnsDbSet(refreshTokens);
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()));
            authenticationDBContextMock.Setup(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()));

            var hasherMock = new Mock<IHasher>();
            hasherMock.Setup(mock => mock.GenerateSalt(It.IsAny<int>())).Returns(_salt);
            hasherMock.Setup(mock => mock.HashString(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(_hashedString);

            var tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);

            // Act
            var result = await tokenService.ValidateRefreshTokenAsync(userId, refreshToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_NoTokensForThisUser_ShouldReturnFalse()
        {
            // Arrange
            var userId = USER_ID;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokens = new List<RefreshToken>
            {
                new()
                {
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(1),
                    IsRevoked = false,
                    IsUsed = false,
                    UserId = "Test",
                    Value = REFRESH_TOKEN_VALUE
                }
            };

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens).ReturnsDbSet(refreshTokens);
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()));
            authenticationDBContextMock.Setup(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()));

            var hasherMock = new Mock<IHasher>();
            hasherMock.Setup(mock => mock.GenerateSalt(It.IsAny<int>())).Returns(_salt);
            hasherMock.Setup(mock => mock.HashString(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(_hashedString);

            var tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);

            // Act
            var result = await tokenService.ValidateRefreshTokenAsync(userId, refreshToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_NoTokensWithSpecificValue_ShouldReturnFalse()
        {
            // Arrange
            var userId = USER_ID;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokens = new List<RefreshToken>
            {
                new()
                {
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(1),
                    IsRevoked = false,
                    IsUsed = false,
                    UserId = USER_ID,
                    Value = "Test"
                }
            };

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens).ReturnsDbSet(refreshTokens);
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()));
            authenticationDBContextMock.Setup(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()));

            var hasherMock = new Mock<IHasher>();
            hasherMock.Setup(mock => mock.GenerateSalt(It.IsAny<int>())).Returns(_salt);
            hasherMock.Setup(mock => mock.HashString(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(_hashedString);

            var tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);

            // Act
            var result = await tokenService.ValidateRefreshTokenAsync(userId, refreshToken);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_TokenExpired_ShouldReturnFalse()
        {
            // Arrange
            var userId = USER_ID;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokens = new List<RefreshToken>
            {
                new()
                {
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(-1),
                    IsRevoked = false,
                    IsUsed = false,
                    UserId = USER_ID,
                    Value = REFRESH_TOKEN_VALUE
                }
            };

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens).ReturnsDbSet(refreshTokens);
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()));
            authenticationDBContextMock.Setup(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()));

            var hasherMock = new Mock<IHasher>();
            hasherMock.Setup(mock => mock.GenerateSalt(It.IsAny<int>())).Returns(_salt);
            hasherMock.Setup(mock => mock.HashString(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(_hashedString);

            var tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);

            // Act
            var result = await tokenService.ValidateRefreshTokenAsync(userId, refreshToken);

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
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tokenService.ValidateRefreshTokenAsync(userId, refreshToken));
        }

        [Fact]
        public async Task ValidateRefreshTokenAsync_RefreshTokenIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            string userId = null;
            var refreshToken = "RefreshToken";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tokenService.ValidateRefreshTokenAsync(userId, refreshToken));
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_CorrectFlow_TokenIsRevoked()
        {
            // Arrange
            var userId = USER_ID;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken()
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                UserId = USER_ID,
                Value = REFRESH_TOKEN_VALUE
            };

            var refreshTokens = new List<RefreshToken>
            {
                refreshTokenObject
            };

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens).ReturnsDbSet(refreshTokens);
            authenticationDBContextMock.Setup(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()));

            var hasherMock = new Mock<IHasher>();

            var tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);

            // Act
            await tokenService.RevokeRefreshTokenAsync(userId, refreshToken);

            // Assert
            refreshTokenObject.IsRevoked.Should().BeTrue();
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_CorrectFlow_SaveChangesIsCalledOnce()
        {
            // Arrange
            var userId = USER_ID;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken()
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                UserId = USER_ID,
                Value = REFRESH_TOKEN_VALUE
            };

            var refreshTokens = new List<RefreshToken>
            {
                refreshTokenObject
            };

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens).ReturnsDbSet(refreshTokens);
            authenticationDBContextMock.Setup(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()));

            var hasherMock = new Mock<IHasher>();

            var tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);

            // Act
            await tokenService.RevokeRefreshTokenAsync(userId, refreshToken);

            // Assert
            authenticationDBContextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_UserIdIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            string userId = null;
            var refreshToken = "RefreshToken";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tokenService.RevokeRefreshTokenAsync(userId, refreshToken));
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_RefreshTokenIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var userId = "test";
            string refreshToken = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _tokenService.RevokeRefreshTokenAsync(userId, refreshToken));
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_NoTokensForThisUser_SaveChangesIsNotCalled()
        {
            // Arrange
            var userId = USER_ID;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokens = new List<RefreshToken>
            {
                new()
                {
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(1),
                    IsRevoked = false,
                    IsUsed = false,
                    UserId = "Test",
                    Value = REFRESH_TOKEN_VALUE
                }
            };

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens).ReturnsDbSet(refreshTokens);
            authenticationDBContextMock.Setup(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()));

            var hasherMock = new Mock<IHasher>();

            var tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);

            // Act
            await tokenService.RevokeRefreshTokenAsync(userId, refreshToken);

            // Assert
            authenticationDBContextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_NoTokensWithSpecificValue_SaveChangesIsNotCalled()
        {
            // Arrange
            var userId = USER_ID;
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokens = new List<RefreshToken>
            {
                new()
                {
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(1),
                    IsRevoked = false,
                    IsUsed = false,
                    UserId = USER_ID,
                    Value = "Test"
                }
            };

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens).ReturnsDbSet(refreshTokens);
            authenticationDBContextMock.Setup(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()));

            var hasherMock = new Mock<IHasher>();

            var tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);

            // Act
            await tokenService.RevokeRefreshTokenAsync(userId, refreshToken);

            // Assert
            authenticationDBContextMock.Verify(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetRefreshTokenAsync_CorrectFlow_ReturnsRefreshToken()
        {
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken()
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                UserId = USER_ID,
                Value = REFRESH_TOKEN_VALUE
            };

            var refreshTokens = new List<RefreshToken>
            {
                refreshTokenObject
            };

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens).ReturnsDbSet(refreshTokens);

            var hasherMock = new Mock<IHasher>();

            var tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);

            // Act
            var result = await tokenService.GetRefreshTokenAsync(refreshToken);

            // Assert
            result.Should().NotBeNull().And.BeEquivalentTo(refreshTokenObject);
        }

        [Fact]
        public async Task GetRefreshTokenAsync_NotTokensWithSpecificValue_ReturnsNull()
        {
            var refreshToken = REFRESH_TOKEN_VALUE;

            var refreshTokenObject = new RefreshToken()
            {
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(1),
                IsRevoked = false,
                IsUsed = false,
                UserId = USER_ID,
                Value = "Test"
            };

            var refreshTokens = new List<RefreshToken>
            {
                refreshTokenObject
            };

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens).ReturnsDbSet(refreshTokens);

            var hasherMock = new Mock<IHasher>();

            var tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);

            // Act
            var result = await tokenService.GetRefreshTokenAsync(refreshToken);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetValidRefreshTokensByUserAsync_CorrectFlowMultipleTokens_ReturnsMultipleValidRefreshToken()
        {
            var refreshTokens = new List<RefreshToken>
            {
                new()
                {
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(1),
                    IsRevoked = false,
                    IsUsed = false,
                    UserId = USER_ID,
                    Value = "Test"
                },
                new()
                {
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    ExpiresAt = DateTime.UtcNow.AddDays(-1),
                    IsRevoked = false,
                    IsUsed = false,
                    UserId = USER_ID,
                    Value = "Test2"
                }
            };

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens).ReturnsDbSet(refreshTokens);

            var hasherMock = new Mock<IHasher>();

            var tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);

            // Act
            var result = await tokenService.GetValidRefreshTokensByUserAsync(USER_ID);

            // Assert
            result.Should().NotBeNull().And.BeEquivalentTo(refreshTokens);
            result.Count.Should().Be(refreshTokens.Count);
        }

        [Fact]
        public async Task GetValidRefreshTokensByUserAsync_NoValidTokens_ReturnsEmptyList()
        {
            var refreshTokens = new List<RefreshToken>
            {
                new()
                {
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(1),
                    IsRevoked = false,
                    IsUsed = true,
                    UserId = USER_ID,
                    Value = "Test"
                },
                new()
                {
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    ExpiresAt = DateTime.UtcNow.AddDays(-1),
                    IsRevoked = true,
                    IsUsed = false,
                    UserId = USER_ID,
                    Value = "Test2"
                }
            };

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens).ReturnsDbSet(refreshTokens);

            var hasherMock = new Mock<IHasher>();

            var tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);

            // Act
            var result = await tokenService.GetValidRefreshTokensByUserAsync(USER_ID);

            // Assert
            result.Should().NotBeNull().And.BeEmpty();
            result.Count.Should().Be(0);
        }
    }
}
