using Application.Authentication.Commands.RequestResetPassword;
using Application.Common.Exceptions;

namespace Application.FunctionalTests.Authentication.Commands.RequestResetPassword
{
    public class RequestResetPasswordCommandValidatorTests : BaseTestFixture
    {
        [Test]
        public void Handle_EmailAddressIsNull_ThrowsValidationException()
        {
            // Arrange
            var command = new RequestResetPasswordCommand
            {
                EmailAddress = null
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_EmailAddressIsEmpty_ThrowsValidationException()
        {
            // Arrange
            var command = new RequestResetPasswordCommand
            {
                EmailAddress = string.Empty
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }

        [Test]
        public void Handle_EmailAddressIsInvalidFormat_ThrowsValidationException()
        {
            // Arrange
            var command = new RequestResetPasswordCommand
            {
                EmailAddress = "invalid-email-format"
            };

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(async () =>
            {
                await SendAsync(command);
            });
        }
    }
}
