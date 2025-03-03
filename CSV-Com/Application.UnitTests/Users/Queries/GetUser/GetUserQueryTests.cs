using Application.Common.Interfaces.CVS;
using Moq;

namespace Application.UnitTests.Users.Queries.GetUser
{
    public class GetUserQueryTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public GetUserQueryTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task Handle_CorrectFlow_ShouldReturnSuccessResultWithUser()
        {
            // Arrange
            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), It.IsAny<string>(), default)).ReturnsAsync();

            // Act


            // Assert

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
