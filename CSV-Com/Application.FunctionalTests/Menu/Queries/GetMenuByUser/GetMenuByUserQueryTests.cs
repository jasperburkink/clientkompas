using Application.Menu.Queries.GetMenuByUser;
using Domain.Authentication.Constants;

namespace Application.FunctionalTests.Menu.Queries.GetMenuByUser
{
    public class GetMenuByUserQueryTests : BaseTestFixture
    {
        [Test]
        public async Task Handle_CorrectFlow_ShouldReturnDto()
        {
            // Arrange
            var userId = await RunAsAsync(Roles.Administrator);

            var query = new GetMenuByUserQuery
            {
                UserId = userId
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull();
            result.MenuItems.Should().NotBeNullOrEmpty();
            result.Role.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Handle_UserNotLoggedIn_ShouldThrowNotAuthorizedException()
        {
            // Arrange
            var query = new GetMenuByUserQuery
            {
                UserId = Guid.NewGuid().ToString()
            };

            // Act
            var act = () => SendAsync(query);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
