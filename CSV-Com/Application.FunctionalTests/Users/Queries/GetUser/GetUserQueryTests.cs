using Application.Common.Mappings;
using Application.Users.Queries.GetUser;
using AutoMapper;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using TestData;
using TestData.User;

namespace Application.FunctionalTests.Users.Queries.GetUser
{
    public class GetUserQueryTests : BaseTestFixture
    {
        private ITestDataGenerator<User> _userTestDataGenerator;
        private IMapper _mapper;

        [SetUp]
        public void Initialize()
        {
            _userTestDataGenerator = new UserDataGenerator();

            var configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = configuration.CreateMapper();
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldReturnSuccessResult()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var user = _userTestDataGenerator.Create();
            await AddAsync(user);

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Value.Should().NotBeNull().And.BeEquivalentTo(userDto);
        }

        [Test]
        public async Task Handle_UserDoesNotExists_ShouldReturnSuccessResult()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var user = _userTestDataGenerator.Create();

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain(GetUserQueryErrors.UserNotFound);
        }

        [Test]
        public async Task Handle_UserNotLoggedIn_ShouldThrowUnAuthorizedException()
        {
            // Arrange
            var user = _userTestDataGenerator.Create();
            await AddAsync(user);

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var act = () => SendAsync(query);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
