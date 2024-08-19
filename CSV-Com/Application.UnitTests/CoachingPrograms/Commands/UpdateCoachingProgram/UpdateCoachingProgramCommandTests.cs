using System.Linq.Expressions;
using Application.CoachingPrograms.Commands.UpdateCoachingProgram;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Interfaces.CVS;
using AutoMapper;
using Domain.CVS.Domain;
using Moq;
using TestData;
using TestData.Client;
using TestData.CoachingProgram.Commands;
using TestData.Organization;

namespace Application.UnitTests.CoachingPrograms.Commands.UpdateCoachingProgram
{
    public class UpdateCoachingProgramCommandTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;
        private readonly UpdateCoachingProgramCommandHandler _handler;
        private readonly UpdateCoachingProgramCommand _command;
        private readonly CoachingProgram _coachingProgram;
        private readonly Client _client;
        private readonly Organization _organization;

        public UpdateCoachingProgramCommandTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uw => uw.ClientRepository.ExistsAsync(
                It.IsAny<Expression<Func<Client, bool>>>(),
                    It.IsAny<CancellationToken>()
                )).ReturnsAsync(true);
            _unitOfWorkMock.Setup(uw => uw.OrganizationRepository.ExistsAsync(
               It.IsAny<Expression<Func<Organization, bool>>>(),
                   It.IsAny<CancellationToken>()
               )).ReturnsAsync(true);

            _mapperMock = new Mock<IMapper>();

            _resourceMessageProviderMock = new Mock<IResourceMessageProvider>();
            _resourceMessageProviderMock
                .Setup(m => m.GetMessage(It.IsAny<Type>(), It.IsAny<string>(), It.IsAny<object[]>()))
                .Returns((Type type, string key, object[] args) =>
                    $"Mock validation message for Type: {type.Name}, Key: {key}, Args: {string.Join(", ", args ?? [])}");

            ITestDataGenerator<Client> clientDataGenerator = new ClientDataGenerator();
            _client = clientDataGenerator.Create();

            ITestDataGenerator<Organization> organizationDataGenerator = new OrganizationDataGenerator();
            _organization = organizationDataGenerator.Create();

            ITestDataGenerator<UpdateCoachingProgramCommand> testDataGenerator = new UpdateCoachingProgramCommandDataGenerator();
            _command = testDataGenerator.Create();
            _command.ClientId = _client.Id;
            _command.OrganizationId = _organization.Id;

            _handler = new UpdateCoachingProgramCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);

            _coachingProgram = new CoachingProgram()
            {
                Id = 1,
                ClientId = _client.Id,
                Client = _client,
                Title = _command.Title,
                OrderNumber = _command.OrderNumber,
                OrganizationId = _organization.Id,
                Organization = _organization,
                CoachingProgramType = _command.CoachingProgramType,
                BeginDate = _command.BeginDate,
                EndDate = _command.EndDate,
                BudgetAmmount = _command.BudgetAmmount,
                HourlyRate = _command.HourlyRate
            };

            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()
                )).ReturnsAsync(_coachingProgram);
            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.InsertAsync(It.IsAny<CoachingProgram>(), default));
            _unitOfWorkMock.Setup(uw => uw.SaveAsync(default)).Returns(Task.CompletedTask);

            _mapperMock.Setup(m => m.Map<UpdateCoachingProgramCommandDto>(It.IsAny<CoachingProgram>())).Returns(new UpdateCoachingProgramCommandDto
            {
                Id = _coachingProgram.Id,
                ClientFullName = _coachingProgram.Client!.FullName,
                Title = _coachingProgram.Title,
                OrderNumber = _coachingProgram.OrderNumber,
                OrganizationName = _coachingProgram.Organization!.OrganizationName,
                CoachingProgramType = _coachingProgram.CoachingProgramType,
                BeginDate = _coachingProgram.BeginDate,
                EndDate = _coachingProgram.EndDate,
                BudgetAmmount = _coachingProgram.BudgetAmmount,
                HourlyRate = _coachingProgram.HourlyRate,
                RemainingHours = _coachingProgram.RemainingHours
            });
        }

        [Fact]
        public async Task Handle_SuccessPath_ShouldReturnCoachingProgramDto()
        {
            // Act
            var coachingProgramDtoResult = await _handler.Handle(_command, default);

            // Assert
            coachingProgramDtoResult.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_CoachingProgramDoesNotExists_ThrowsNotFoundException()
        {
            // Arrange
            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()
                )).Returns(Task.FromResult<CoachingProgram>(null));

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(_command, default));
        }
    }
}
