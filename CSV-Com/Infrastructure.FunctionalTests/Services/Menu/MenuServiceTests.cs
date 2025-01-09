using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Domain.Authentication.Constants;
using FluentAssertions;
using Infrastructure.Services;
using Infrastructure.Services.Menu;

namespace Infrastructure.FunctionalTests.Services.Menu
{
    public class MenuServiceTests
    {
        private readonly IFileService _fileService;

        public MenuServiceTests()
        {
            _fileService = new FileService();
        }

        [Fact]
        public void GetMenuByRole_RoleIsLicensee_ReturnMenuItems()
        {
            // Arrange
            IMenuService menuService = new MenuService(_fileService);

            // Act
            var result = menuService.GetMenuByRole(Roles.Licensee);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void GetMenuByRole_RoleDoesNotExists_ReturnEmptyList()
        {
            // Arrange
            IMenuService menuService = new MenuService(_fileService);

            // Act
            var result = menuService.GetMenuByRole("RoleDoesNotExists");

            // Assert
            result.Should().NotBeNull().And.BeEmpty();
        }
    }
}
