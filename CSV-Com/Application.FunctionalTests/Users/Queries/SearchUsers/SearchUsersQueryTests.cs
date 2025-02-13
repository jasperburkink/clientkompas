using Application.Users.Queries.SearchUsers;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using Infrastructure.Data.Authentication;
using Infrastructure.Identity;
using TestData;
using TestData.User;

namespace Application.FunctionalTests.Users.Queries.SearchUsers

{
    public class SearchUsersQueryTests : BaseTestFixture
    {
        private User _user1, _user2;
        private AuthenticationUser _authUser1, _authUser2;

        [SetUp]
        public async Task Initialize()
        {
            var roles = new List<string> { Roles.SystemOwner, Roles.Licensee, Roles.Administrator, Roles.Coach }
                        .Except(await Testing.IdentityService.GetAvailableUserRolesAsync());

            foreach (var role in roles)
            {
                await AddAsync<AuthenticationRole, AuthenticationDbContext>(new AuthenticationRole(role) { NormalizedName = role.ToUpper() });
            }

            ITestDataGenerator<User> userDataGenerator = new UserDataGenerator();
            _user1 = userDataGenerator.Create();
            await AddAsync(_user1);
            _user2 = userDataGenerator.Create();
            await AddAsync(_user2);

            _authUser1 = new AuthenticationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = _user1.EmailAddress,
                Email = _user1.EmailAddress,
                CVSUserId = _user1.Id
            };
            await AddAsync<AuthenticationUser, AuthenticationDbContext>(_authUser1);

            _authUser2 = new AuthenticationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = _user2.EmailAddress,
                Email = _user2.EmailAddress,
                CVSUserId = _user2.Id
            };
            await AddAsync<AuthenticationUser, AuthenticationDbContext>(_authUser2);
        }

        [Test]
        public async Task Handle_SearchByRightFirstName_ReturnsUsers()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);
            var query = new SearchUsersQuery { SearchTerm = _user1.FirstName };

            // Act
            var users = await SendAsync(query);

            // Assert
            users.Should().Contain(u => u.Id == _user1.Id);
        }

        [Test]
        public async Task Handle_SearchByWrongFirstName_ReturnsNoUsers()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);
            var query = new SearchUsersQuery { SearchTerm = "WrongName" };

            // Act
            var users = await SendAsync(query);

            // Assert
            users.Should().BeEmpty();
        }

        [Test]
        public async Task Handle_SystemOwnerIsExcludedFromResults_ReturnsNoSystemOwner()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);
            await Testing.IdentityService.AddUserToRoleAsync(_authUser1.Id, Roles.SystemOwner);
            await Testing.IdentityService.AddUserToRoleAsync(_authUser2.Id, Roles.Coach);

            var user1Dto = new SearchUsersQueryDto
            {
                Id = _user1.Id,
                FirstName = _user1.FirstName,
                FullName = _user1.FullName,
                IsDeactivated = _user1.IsDeactivated,
                LastName = _user1.LastName,
                PrefixLastName = _user1.PrefixLastName
            };

            var user2Dto = new SearchUsersQueryDto
            {
                Id = _user2.Id,
                FirstName = _user2.FirstName,
                FullName = _user2.FullName,
                IsDeactivated = _user2.IsDeactivated,
                LastName = _user2.LastName,
                PrefixLastName = _user2.PrefixLastName
            };

            var query = new SearchUsersQuery { SearchTerm = string.Empty };

            // Act
            var users = await SendAsync(query);

            // Assert
            users.Count().Should().Be(1);
            users.Should().NotContainEquivalentOf(user1Dto).And.ContainEquivalentOf(user2Dto);
        }

        [Test]
        public async Task Handle_Unauthorized_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var query = new SearchUsersQuery { SearchTerm = "WrongName" };

            // Act
            var act = async () => await SendAsync(query);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
