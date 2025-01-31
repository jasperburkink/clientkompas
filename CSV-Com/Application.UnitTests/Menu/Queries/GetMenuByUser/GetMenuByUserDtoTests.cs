using Application.Common.Interfaces.Authentication;
using Application.Menu.Queries.GetMenuByUser;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using Moq;

namespace Application.UnitTests.Menu.Queries.GetMenuByUser
{
    public class GetMenuByUserDtoTests
    {
        private readonly GetMenuByUserQuery _query;
        private readonly Mock<IIdentityService> _identityServiceMock;
        private readonly Mock<IMenuService> _menuServiceMock;

        public GetMenuByUserDtoTests()
        {
            _query = new GetMenuByUserQuery
            {
                UserId = Guid.NewGuid().ToString()
            };

            var roles = new List<string>()
            {
                Roles.SystemOwner,
                Roles.Licensee,
                Roles.Administrator,
                Roles.Coach
            };

            _identityServiceMock = new Mock<IIdentityService>();
            _identityServiceMock.Setup(mock => mock.GetRolesAsync(It.IsAny<string>())).ReturnsAsync(roles);

            var menuItems = new List<MenuItem>
            {
                new()
                {
                    Icon = "Users",
                    Text = "Gebruikers",
                    To = "/Users"
                },
                new()
                {
                    Id = "Logout",
                    Icon = "Exit",
                    Text = "Uitloggen",
                    To = "/Logout"
                }
            };

            _menuServiceMock = new Mock<IMenuService>();
            _menuServiceMock.Setup(mock => mock.GetMenuByRole(It.IsAny<string>())).Returns(menuItems);
        }

        [Fact]
        public async Task Handle_CorrectFlow_MenuItemsIsNotEmpty()
        {
            // Arrange
            var handler = new GetMenuByUserHandler(_identityServiceMock.Object, _menuServiceMock.Object);

            // Act
            var result = await handler.Handle(_query, default);

            // Assert
            result.MenuItems.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Handle_CorrectFlow_RoleIsNotEmpty()
        {
            // Arrange
            var handler = new GetMenuByUserHandler(_identityServiceMock.Object, _menuServiceMock.Object);

            // Act
            var result = await handler.Handle(_query, default);

            // Assert
            result.Role.Should().NotBeNullOrEmpty();
        }
    }
}
