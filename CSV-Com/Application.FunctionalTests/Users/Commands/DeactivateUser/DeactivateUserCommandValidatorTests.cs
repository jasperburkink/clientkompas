using Application.Common.Exceptions;
using Application.Users.Commands.DeactivateUser;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using TestData;
using TestData.User;

namespace Application.FunctionalTests.Users.Commands.DeactivateUser
{
    public class DeactivateUserCommandValidatorTests
    {
        private ITestDataGenerator<User> _testDataGeneratorUser;

        [SetUp]
        public void SetUp()
        {
            _testDataGeneratorUser = new UserDataGenerator();
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldReturnSuccesResult()
        {
            // Arrange
            var user = _testDataGeneratorUser.Create();

            await RunAsAsync(Roles.Administrator);

            await AddAsync(user);

            var command = new DeactivateUserCommand
            {
                Id = user.Id
            };

            // Act
            var result = await SendAsync(command);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
        }

        [Test]
        public async Task Handle_UserDoesNotExist_ShouldThrowValidationException()
        {
            // Arrange
            var user = _testDataGeneratorUser.Create();

            await RunAsAsync(Roles.Administrator);

            var command = new DeactivateUserCommand
            {
                Id = user.Id
            };

            // Act
            var act = () => SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
