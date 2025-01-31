using Application.Users.Queries.GetUserRoles;
using Domain.Authentication.Constants;

namespace Application.FunctionalTests.Users.Queries.GetUserRoles
{
    public class GetUserRolesQueryTests : BaseTestFixture
    {
        [Test]
        public async Task Handle_GetUserRoles_ShouldReturnRoles()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new GetUserRolesQuery();

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull().And.NotBeEmpty();
        }

        [Test]
        public async Task Handle_NotLoggedIn_ShouldUnauthorizedException()
        {
            // Arrange
            var query = new GetUserRolesQuery();

            // Act
            var act = async () => await SendAsync(query);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
