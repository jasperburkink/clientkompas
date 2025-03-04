using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using Application.Users.Queries.GetUser;
using AutoMapper;
using Domain.CVS.Domain;
using Moq;
using TestData;
using TestData.User;

namespace Application.UnitTests.Users.Queries.GetUser
{
    public class GetUserQueryTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly ITestDataGenerator<User> _userTestDataGenerator;
        private readonly IMapper _mapper;

        public GetUserQueryTests()
        {
            _userTestDataGenerator = new UserDataGenerator();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public async Task Handle_CorrectFlow_ShouldReturnSuccessResultWithUser()
        {
            // Arrange
            var user = _userTestDataGenerator.Create();

            var userDto = _mapper.Map<GetUserQueryDto>(user);

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync(user);

            var handler = new GetUserQueryHandler(_unitOfWorkMock.Object, _mapper);

            var query = new GetUserQuery { UserId = user.Id };

            // Act
            var result = await handler.Handle(query, default);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
            result.Value.Should().BeEquivalentTo(userDto);
        }

        [Fact]
        public async Task Handle_UserDoesNotExists_ShouldReturnFailureResult()
        {
            // Arrange


            // Act


            // Assert

        }
    }
}
