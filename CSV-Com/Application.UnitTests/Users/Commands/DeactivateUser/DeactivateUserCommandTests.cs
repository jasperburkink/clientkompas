using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using Application.Users.Commands.DeactivateUser;
using AutoMapper;
using Domain.CVS.Domain;
using Microsoft.EntityFrameworkCore;
using Moq;
using TestData;
using TestData.User;

namespace Application.UnitTests.Users.Commands.DeactivateUser
{
    public class DeactivateUserCommandTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        private readonly ITestDataGenerator<User> _testDataGenerator;
        private readonly Mock<IEmailService> _emailServiceMock;

        public DeactivateUserCommandTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());
            _mapper = _configuration.CreateMapper();

            _testDataGenerator = new UserDataGenerator();

            _emailServiceMock = new Mock<IEmailService>();
            _emailServiceMock.Setup(mock => mock.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public async Task Handle_CorrectFlow_ReturnsResultSuccess()
        {
            // Arrange
            var user = _testDataGenerator.Create();

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), default)).ReturnsAsync(user);

            var command = new DeactivateUserCommand()
            {
                Id = user.Id
            };

            var handler = new DeactivateUserCommandHandler(_unitOfWorkMock.Object, _mapper, _emailServiceMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_UserDoesNotExists_ReturnsFailureResult()
        {
            // Arrange
            var user = _testDataGenerator.Create();

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), default));

            var command = new DeactivateUserCommand()
            {
                Id = user.Id
            };

            var handler = new DeactivateUserCommandHandler(_unitOfWorkMock.Object, _mapper, _emailServiceMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain(DeactivateUserCommandErrors.UserNotFound.WithParams(user.Id));
        }

        [Fact]
        public async Task Handle_UserIsAlreadyDeactivated_ReturnsFailureResult()
        {
            // Arrange
            var now = DateTime.UtcNow;

            var user = _testDataGenerator.Create();
            user.Deactivate(now);

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), default)).ReturnsAsync(user);

            var command = new DeactivateUserCommand()
            {
                Id = user.Id
            };

            var handler = new DeactivateUserCommandHandler(_unitOfWorkMock.Object, _mapper, _emailServiceMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().Contain(DeactivateUserCommandErrors.UserAlreadyDeactivated.WithParams(user.Id, now));
        }

        [Fact]
        public async Task Handle_SaveChangesFailure_ThrowsDbUpdateException()
        {
            // Arrange
            var user = _testDataGenerator.Create();

            _unitOfWorkMock.Setup(mock => mock.UserRepository.GetByIDAsync(It.IsAny<int>(), default)).ReturnsAsync(user);
            _unitOfWorkMock.Setup(uow => uow.SaveAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new DbUpdateException("Failed to save changes"));

            var command = new DeactivateUserCommand()
            {
                Id = user.Id
            };

            var handler = new DeactivateUserCommandHandler(_unitOfWorkMock.Object, _mapper, _emailServiceMock.Object);

            // Act
            var act = () => handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DbUpdateException>();
        }
    }
}
