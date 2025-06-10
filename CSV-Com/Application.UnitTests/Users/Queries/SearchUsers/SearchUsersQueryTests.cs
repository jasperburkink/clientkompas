using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using Application.Users.Queries.SearchUsers;
using AutoMapper;
using Domain.Authentication.Constants;
using Domain.Authentication.Domain;
using Domain.CVS.Domain;
using Infrastructure.Identity;
using Moq;
using TestData;
using TestData.User;

namespace Application.UnitTests.Users.Queries.SearchUsers
{
    public class SearchUsersQueryTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IIdentityService> _identityServiceMock;
        private readonly SearchUsersQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        private readonly User _user1, _user2, _user3, _systemOwner;

        public SearchUsersQueryTests()
        {
            _unitOfWorkMock = new();
            _identityServiceMock = new();

            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _handler = new SearchUsersQueryHandler(_unitOfWorkMock.Object, _identityServiceMock.Object, _mapper);


            ITestDataGenerator<User> testDataGenerator = new UserDataGenerator();

            _user1 = testDataGenerator.Create();
            _user1.Id = 1;
            _user2 = testDataGenerator.Create();
            _user2.Id = 2;
            _user3 = testDataGenerator.Create();
            _user3.Id = 3;
            _systemOwner = testDataGenerator.Create();
            _systemOwner.Id = 4;
        }

        [Fact]
        public async Task Handle_SearchUsers_ShouldReturnUserDtos()
        {
            // Arrange
            var query = new SearchUsersQuery { SearchTerm = "Alice" };

            var users = new List<User> { _user1, _user2 }.AsQueryable();

            var userDtos = users.Select(u => new SearchUsersQueryDto
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                FullName = u.FullName,
                PrefixLastName = u.PrefixLastName,
                DeactivationDateTime = u.DeactivationDateTime
            }).ToList();

            _unitOfWorkMock.Setup(uw => uw.UserRepository.FullTextSearch(
                query.SearchTerm, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<User, object>>>()
            )).ReturnsAsync(users);

            _identityServiceMock.Setup(s => s.GetUsersInRolesAsync(Roles.SystemOwner))
                .ReturnsAsync([]);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(userDtos);
        }

        [Fact]
        public async Task Handle_NoMatchingUsers_ShouldReturnEmptyList()
        {
            // Arrange
            var query = new SearchUsersQuery { SearchTerm = "NonExisting" };

            _unitOfWorkMock.Setup(uw => uw.UserRepository.FullTextSearch(
                query.SearchTerm, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<User, object>>>()
            )).ReturnsAsync([]);

            _identityServiceMock.Setup(s => s.GetUsersInRolesAsync(Roles.SystemOwner))
                .ReturnsAsync([]);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_ShouldFilterOutSystemOwners_ShouldReturnSystemOwners()
        {
            // Arrange
            var query = new SearchUsersQuery { SearchTerm = string.Empty };

            var users = new List<User>
            {
                _user1,
                _user2,
                _systemOwner
            }.AsQueryable();

            _unitOfWorkMock.Setup(uw => uw.UserRepository.FullTextSearch(
                query.SearchTerm, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<User, object>>>()
            )).ReturnsAsync(users);

            var systemOwners = new List<IAuthenticationUser> { new AuthenticationUser() { CVSUserId = _systemOwner.Id } };
            _identityServiceMock.Setup(s => s.GetUsersInRolesAsync(Roles.SystemOwner))
                .ReturnsAsync(systemOwners);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotContain(u => u.Id == _systemOwner.Id);
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handle_SearchUsers_ShouldReturnSortedDtos()
        {
            // Arrange
            var query = new SearchUsersQuery { SearchTerm = "e" };

            _user1.FirstName = "Jane";
            _user1.LastName = "Doe";
            _user2.FirstName = "John";
            _user2.LastName = "Doe";
            _user3.FirstName = "Alice";
            _user3.LastName = "Smith";

            var users = new List<User>
            {
                _user3,
                _user2,
                _user1
            }.AsQueryable();

            var userDtos = users
                .Where(u => u.FullName.Contains(query.SearchTerm))
                .Select(u => new SearchUsersQueryDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    FullName = u.FullName,
                    PrefixLastName = u.PrefixLastName,
                })
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToList();

            _unitOfWorkMock.Setup(uw => uw.UserRepository.FullTextSearch(
                query.SearchTerm, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<User, object>>>()
            )).ReturnsAsync(users);

            _identityServiceMock.Setup(s => s.GetUsersInRolesAsync(Roles.SystemOwner))
                .ReturnsAsync([]);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(userDtos);
            result.First().FirstName.Should().Be("Jane");
            result.Last().FirstName.Should().Be("Alice");
        }
    }
}
