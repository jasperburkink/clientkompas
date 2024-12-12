using Application.Authentication.Commands.TwoFactorAuthentication;
using Application.Common.Exceptions;

namespace Application.FunctionalTests.Authentication.Commands.TwoFactorAuthentication
{
    public class TwoFactorAuthenticationCommandValidatorTests : BaseTestFixture
    {
        private TwoFactorAuthenticationCommand _command;

        [SetUp]
        public async Task SetUp()
        {
            UseMocks = true;

            _command = new TwoFactorAuthenticationCommand
            {
                UserId = CustomWebApplicationFactoryWithMocks.AuthenticationUser.Id,
                Token = "321654"
            };
        }

        [Test]
        public async Task Handle_CorrectFlow_NoValidationErrors()
        {
            // Act
            Func<Task<TwoFactorAuthenticationCommandDto>> act = async () => await SendAsync(_command);

            // Assert
            await act.Should().NotThrowAsync<ValidationException>();
        }

        [Test]
        public async Task Handle_UserIdIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                UserId = null
            };

            // Act
            Func<Task<TwoFactorAuthenticationCommandDto>> act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task Handle_UserIdIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                UserId = string.Empty
            };

            // Act
            Func<Task<TwoFactorAuthenticationCommandDto>> act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task Handle_TokenIsNull_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                Token = null
            };

            // Act
            Func<Task<TwoFactorAuthenticationCommandDto>> act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task Handle_TokenIsEmpty_ShouldThrowValidationException()
        {
            // Arrange
            var command = _command with
            {
                Token = string.Empty
            };

            // Act
            Func<Task<TwoFactorAuthenticationCommandDto>> act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }

        [TearDown]
        public void TearDown() => UseMocks = false;
    }
}
