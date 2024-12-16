using Application.Authentication.Commands.RefreshToken;
using Application.Common.Exceptions;

namespace Application.FunctionalTests.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandValidatorTests : BaseTestFixture
    {
        [Test]
        public void Handle_RequestRefreshTokenIsNull_ThrowsValidationException()
        {
            // Arrange
            var command = new RefreshTokenCommand
            {
                RefreshToken = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_RequestRefreshTokenIsEmpty_ThrowsValidationException()
        {
            // Arrange
            var command = new RefreshTokenCommand
            {
                RefreshToken = string.Empty
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }
    }
}
