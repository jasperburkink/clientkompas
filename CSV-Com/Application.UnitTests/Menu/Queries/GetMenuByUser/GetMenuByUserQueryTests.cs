using Application.Common.Interfaces.Authentication;
using Application.Menu.Queries.GetMenuByUser;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using Moq;

namespace Application.UnitTests.Menu.Queries.GetMenuByUser
{
    public class GetMenuByUserQueryTests
    {
        private readonly GetMenuByUserQuery _query;
        private readonly Mock<IIdentityService> _identityServiceMock;
        private readonly Mock<IMenuService> _menuServiceMock;

        public GetMenuByUserQueryTests()
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
        public async Task Handle_CorrectFlow_ReturnsFilledDto()
        {
            // Arrange
            var handler = new GetMenuByUserHandler(_identityServiceMock.Object, _menuServiceMock.Object);

            // Act
            var result = await handler.Handle(_query, default);

            // Assert
            result.Should().NotBeNull();
            result.MenuItems.Should().NotBeNullOrEmpty();
            result.Role.Should().NotBeNullOrEmpty();
        }

        [Theory]
        [InlineData(Roles.SystemOwner)]
        [InlineData(Roles.Licensee)]
        [InlineData(Roles.Administrator)]
        [InlineData(Roles.Coach)]
        public async Task Handle_UserHasRole_RoleShouldBeCorrectRole(string role)
        {
            // Arrange
            var roles = new List<string>()
            {
                role
            };

            _identityServiceMock.Setup(mock => mock.GetRolesAsync(It.IsAny<string>())).ReturnsAsync(roles);

            var handler = new GetMenuByUserHandler(_identityServiceMock.Object, _menuServiceMock.Object);

            // Act
            var result = await handler.Handle(_query, default);

            // Assert
            result.Role.Should().Be(role);
        }
    }
}
