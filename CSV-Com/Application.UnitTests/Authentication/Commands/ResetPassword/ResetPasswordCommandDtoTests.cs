using Application.Authentication.Commands.ResetPassword;
using Application.Common.Interfaces.Authentication;
using Application.Common.Models;
using Moq;
using TestData;

namespace Application.UnitTests.Authentication.Commands.ResetPassword
{
    public class ResetPasswordCommandDtoTests
    {
        [Fact]
        public async Task Handle_EmailAddressIsSet_ResultSuccessIsTrue()
        {
            // Arrange
            var password = FakerConfiguration.Faker.Internet.Password();

            var identityServiceMock = new Mock<IIdentityService>();
            identityServiceMock.Setup(mock => mock.ResetPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(Result.Success);

            var handler = new ResetPasswordCommandHandler(identityServiceMock.Object);

            var command = new ResetPasswordCommand
            {
                EmailAddress = FakerConfiguration.Faker.Person.Email,
                Token = "Token",
                NewPassword = password,
                NewPasswordRepeat = password
            };

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            result.Success.Should().BeTrue();
        }

        Hier ben ik gebleven.
    }
}
