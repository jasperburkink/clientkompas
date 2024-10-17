using System.Linq.Expressions;
using System.Text;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Domain;
using FluentAssertions;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Infrastructure.UnitTests.Identity
{
    public class RefreshTokenServiceTests
    {
        [Fact]
        public async Task GenerateRefreshTokenAsync_CorrectFlow_ShouldGenerateValidToken()
        {
            // Arrange
            var hashedString = "TestHash";
            var user = new AuthenticationUser
            {
                Id = "123",
                UserName = "testuser",
                Email = "test@example.com",
                CVSUserId = 1,
                Salt = Encoding.UTF8.GetBytes("Salt")
            };

            var authenticationDBContextMock = new Mock<IAuthenticationDbContext>();
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens.AddAsync(It.IsAny<IRefreshToken>(), It.IsAny<CancellationToken>()));
            authenticationDBContextMock.Setup(mock => mock.SaveChangesAsync(It.IsAny<CancellationToken>()));
            authenticationDBContextMock.Setup(mock => mock.RefreshTokens.FirstOrDefaultAsync(It.IsAny<Expression<Func<string, bool>>>(), It.IsAny<CancellationToken>()));
            var hasherMock = new Mock<IHasher>();
            hasherMock.Setup(mock => mock.GenerateSalt(It.IsAny<int>())).Returns(user.Salt);
            hasherMock.Setup(mock => mock.HashString(It.IsAny<string>(), It.IsAny<byte[]>())).Returns(hashedString);
            var tokenService = new RefreshTokenService(authenticationDBContextMock.Object, hasherMock.Object);

            // Act
            var token = await tokenService.GenerateRefreshTokenAsync(user);

            // Assert
            token.Should().NotBeNull();
            token.Should().BeOfType<string>();
        }

    }
}
