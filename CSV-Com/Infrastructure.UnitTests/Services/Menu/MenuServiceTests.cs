using Application.Common.Interfaces;
using Domain.Authentication.Constants;
using FluentAssertions;
using Infrastructure.Services.Menu;
using Moq;

namespace Infrastructure.UnitTests.Services.Menu
{
    public class MenuServiceTests
    {
        private readonly Mock<IFileService> _fileServiceMock;

        public MenuServiceTests()
        {
            _fileServiceMock = new Mock<IFileService>();
        }

        [Fact]
        public void GetMenuByRole_CorrectFlowMultipleRoles_ReturnsMenuItems()
        {
            // Arrange
            var role = Roles.Coach;

            var json = $@"
                {{
                  ""roles"": {{
                    ""{role}"": [
                      {{
                    ""to"": "" / licences"",
                        ""text"": ""Licenties"",
                        ""icon"": ""Licence"",
                        ""id"": null
                      }},
                      {{
                        ""to"": ""/logout"",
                        ""text"": ""Uitloggen"",
                        ""icon"": ""Exit"",
                        ""id"": ""logout""
                      }}
                    ],
                    ""Role2"": [
                      {{
                        ""to"": ""/logout"",
                        ""text"": ""Uitloggen"",
                        ""icon"": ""Exit"",
                        ""id"": ""logout""
                      }}
                    ]
                  }}
                }}
                ";

            _fileServiceMock.Setup(mock => mock.ReadAllText(It.IsAny<string>())).Returns(json);

            var menuService = new MenuService(_fileServiceMock.Object);

            // Act
            var menuItems = menuService.GetMenuByRole(role);

            // Assert
            menuItems.Should().NotBeNull().And.HaveCount(2);
        }

        [Fact]
        public void GetMenuByRole_RoleDoesNotExists_ReturnsEmptyList()
        {
            // Arrange
            var role = Roles.Coach;

            var json = $@"
                {{
                  ""roles"": {{
                    ""Role1"": [
                      {{
                    ""to"": "" / licences"",
                        ""text"": ""Licenties"",
                        ""icon"": ""Licence"",
                        ""id"": null
                      }},
                      {{
                        ""to"": ""/logout"",
                        ""text"": ""Uitloggen"",
                        ""icon"": ""Exit"",
                        ""id"": ""logout""
                      }}
                    ],
                    ""Role2"": [
                      {{
                        ""to"": ""/logout"",
                        ""text"": ""Uitloggen"",
                        ""icon"": ""Exit"",
                        ""id"": ""logout""
                      }}
                    ]
                  }}
                }}
                ";

            _fileServiceMock.Setup(mock => mock.ReadAllText(It.IsAny<string>())).Returns(json);

            var menuService = new MenuService(_fileServiceMock.Object);

            // Act
            var menuItems = menuService.GetMenuByRole(role);

            // Assert
            menuItems.Should().NotBeNull().And.BeEmpty();
        }
    }
}
