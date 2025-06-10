using Application.Users.Queries.GetUserRoles;
using Domain.Authentication.Constants;

namespace Application.FunctionalTests.Users.Queries.GetUserRoles
{
    public class GetUserRolesDtoTests : BaseTestFixture
    {
        [Test]
        public async Task Handle_GetUserRoles_ShouldContainValues()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new GetUserRolesQuery();

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull().And.NotBeEmpty();
            result.All(dto => !string.IsNullOrWhiteSpace(dto.Value)).Should().BeTrue();
        }

        [Test]
        public async Task Handle_GetUserRoles_ShouldContainNames()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new GetUserRolesQuery();

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull().And.NotBeEmpty();
            result.All(dto => !string.IsNullOrWhiteSpace(dto.Name)).Should().BeTrue();
        }
    }
}
