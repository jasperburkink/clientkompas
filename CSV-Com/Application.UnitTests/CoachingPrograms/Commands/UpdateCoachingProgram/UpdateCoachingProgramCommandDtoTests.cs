using System.Linq.Expressions;
using Application.CoachingPrograms.Commands.UpdateCoachingProgram;
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
    public class UpdateCoachingProgramCommandDtoTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IResourceMessageProvider> _resourceMessageProviderMock;
        private readonly UpdateCoachingProgramCommandHandler _handler;
        private readonly UpdateCoachingProgramCommand _command;
        private readonly CoachingProgram _coachingProgram;
        private readonly Client _client;
        private readonly Organization _organization;

        public UpdateCoachingProgramCommandDtoTests()
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
                CoachingProgramType = _command.CoachingProgramType.Value,
                BeginDate = _command.BeginDate.Value,
                EndDate = _command.EndDate.Value,
                BudgetAmmount = _command.BudgetAmmount,
                HourlyRate = _command.HourlyRate.Value
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
        public async Task Handle_UpdateCoachingProgram_ShouldReturnId()
        {
            // Act
            var coachingProgramDtoResult = await _handler.Handle(_command, default);

            // Assert
            coachingProgramDtoResult.Id.Should().Be(_coachingProgram.Id);
        }

        [Fact]
        public async Task Handle_UpdateCoachingProgram_ShouldReturnClientName()
        {
            // Act
            var coachingProgramDtoResult = await _handler.Handle(_command, default);

            // Assert
            coachingProgramDtoResult.ClientFullName.Should().Be(_client.FullName);
        }

        [Fact]
        public async Task Handle_UpdateCoachingProgram_TitleShouldBeSet()
        {
            // Act
            var coachingProgramDtoResult = await _handler.Handle(_command, default);

            // Assert
            coachingProgramDtoResult.Title.Should().Be(_coachingProgram.Title);
        }

        [Fact]
        public async Task Handle_UpdateCoachingProgram_OrderNumberShouldBeSet()
        {
            // Act
            var coachingProgramDtoResult = await _handler.Handle(_command, default);

            // Assert
            coachingProgramDtoResult.OrderNumber.Should().Be(_coachingProgram.OrderNumber);
        }

        [Fact]
        public async Task Handle_UpdateCoachingProgram_ShouldReturnOrganizationName()
        {
            // Act
            var coachingProgramDtoResult = await _handler.Handle(_command, default);

            // Assert
            coachingProgramDtoResult.OrganizationName.Should().Be(_organization.OrganizationName);
        }

        [Fact]
        public async Task Handle_UpdateCoachingProgram_CoachingProgramTypeShouldBeSet()
        {
            // Act
            var coachingProgramDtoResult = await _handler.Handle(_command, default);

            // Assert
            coachingProgramDtoResult.CoachingProgramType.Should().Be(_coachingProgram.CoachingProgramType);
        }

        [Fact]
        public async Task Handle_UpdateCoachingProgram_BeginDateShouldBeSet()
        {
            // Act
            var coachingProgramDtoResult = await _handler.Handle(_command, default);

            // Assert
            coachingProgramDtoResult.BeginDate.Should().Be(_coachingProgram.BeginDate);
        }

        [Fact]
        public async Task Handle_UpdateCoachingProgram_EndDateShouldBeSet()
        {
            // Act
            var coachingProgramDtoResult = await _handler.Handle(_command, default);

            // Assert
            coachingProgramDtoResult.EndDate.Should().Be(_coachingProgram.EndDate);
        }

        [Fact]
        public async Task Handle_UpdateCoachingProgram_BudgetAmmountShouldBeSet()
        {
            // Act
            var coachingProgramDtoResult = await _handler.Handle(_command, default);

            // Assert
            coachingProgramDtoResult.BudgetAmmount.Should().Be(_coachingProgram.BudgetAmmount);
        }

        [Fact]
        public async Task Handle_UpdateCoachingProgram_HourlyRateShouldBeSet()
        {
            // Act
            var coachingProgramDtoResult = await _handler.Handle(_command, default);

            // Assert
            coachingProgramDtoResult.HourlyRate.Should().Be(_coachingProgram.HourlyRate);
        }

        [Fact]
        public async Task Handle_UpdateCoachingProgram_RemainingHoursShouldBeSet()
        {
            // Act
            var coachingProgramDtoResult = await _handler.Handle(_command, default);

            // Assert
            coachingProgramDtoResult.RemainingHours.Should().Be(_coachingProgram.RemainingHours);
        }
    }
}
