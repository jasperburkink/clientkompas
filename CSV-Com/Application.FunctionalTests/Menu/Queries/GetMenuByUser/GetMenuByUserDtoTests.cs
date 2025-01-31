using Application.Menu.Queries.GetMenuByUser;
using Domain.Authentication.Constants;

namespace Application.FunctionalTests.Menu.Queries.GetMenuByUser
{
    public class GetMenuByUserDtoTests : BaseTestFixture
    {
        [Test]
        public async Task Handle_Role_ShouldBeSet()
        {
            // Arrange
            var role = Roles.Administrator;

            var userId = await RunAsAsync(role);

            var query = new GetMenuByUserQuery
            {
                UserId = userId
            };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Role.Should().Be(role);
        }

        [Test]
        public async Task Handle_MenuItems_ShouldBeSet()
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
            result.MenuItems.Should().NotBeNullOrEmpty();
        }
    }
}
