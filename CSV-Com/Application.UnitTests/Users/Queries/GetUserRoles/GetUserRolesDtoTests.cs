using Application.Users.Queries.GetUserRoles;

namespace Application.UnitTests.Users.Queries.GetUserRoles
{
    public class GetUserRolesDtoTests
    {
        [Fact]
        public void GetUserRolesDto_Properties_ShouldBeSetCorrectly()
        {
            // Arrange
            var dto = new GetUserRolesDto
            {
                Name = "Admin",
                Value = "admin"
            };

            // Assert
            dto.Name.Should().Be("Admin");
            dto.Value.Should().Be("admin");
        }
    }
}
