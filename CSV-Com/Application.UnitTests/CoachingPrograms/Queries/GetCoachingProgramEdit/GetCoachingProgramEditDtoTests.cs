﻿using Application.CoachingPrograms.Queries.GetCoachingProgramEdit;
using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using AutoMapper;
using Moq;
using TestData;
using TestData.CoachingProgram;

namespace Application.UnitTests.CoachingPrograms.Queries.GetCoachingProgramEdit
{
    public class GetCoachingProgramEditDtoTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetCoachingProgramEditQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        private readonly Domain.CVS.Domain.CoachingProgram _coachingProgram;

        public GetCoachingProgramEditDtoTests()
        {
            _unitOfWorkMock = new();
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _handler = new GetCoachingProgramEditQueryHandler(_unitOfWorkMock.Object, _mapper);

            ITestDataGenerator<Domain.CVS.Domain.CoachingProgram> clientDataGenerator = new CoachingProgramDataGenerator(true);
            _coachingProgram = clientDataGenerator.Create();
        }

        [Fact]
        public async Task Handle_GetCoachingProgram_ClientIdShouldBeSet()
        {
            // Arrange
            var query = new GetCoachingProgramEditQuery { Id = _coachingProgram.Id };

            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                query.Id, default
            )).ReturnsAsync(_coachingProgram);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.ClientId.Should().Be(_coachingProgram.ClientId);
        }

        [Fact]
        public async Task Handle_GetCoachingProgram_TitleShouldBeSet()
        {
            // Arrange
            var query = new GetCoachingProgramEditQuery { Id = _coachingProgram.Id };

            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                query.Id, default
            )).ReturnsAsync(_coachingProgram);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Title.Should().Be(_coachingProgram.Title);
        }

        [Fact]
        public async Task Handle_GetCoachingProgram_OrderNumberShouldBeSet()
        {
            // Arrange
            var query = new GetCoachingProgramEditQuery { Id = _coachingProgram.Id };

            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                query.Id, default
            )).ReturnsAsync(_coachingProgram);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.OrderNumber.Should().Be(_coachingProgram.OrderNumber);
        }

        [Fact]
        public async Task Handle_GetCoachingProgram_OrganizationIdShouldBeSet()
        {
            // Arrange
            var query = new GetCoachingProgramEditQuery { Id = _coachingProgram.Id };

            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                query.Id, default
            )).ReturnsAsync(_coachingProgram);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.OrganizationId.Should().Be(_coachingProgram.OrganizationId);
        }

        [Fact]
        public async Task Handle_GetCoachingProgram_CoachingProgramTypeShouldBeSet()
        {
            // Arrange
            var query = new GetCoachingProgramEditQuery { Id = _coachingProgram.Id };

            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                query.Id, default
            )).ReturnsAsync(_coachingProgram);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.CoachingProgramType.Should().Be((int)_coachingProgram.CoachingProgramType);
        }

        [Fact]
        public async Task Handle_GetCoachingProgram_BeginDateShouldBeSet()
        {
            // Arrange
            var query = new GetCoachingProgramEditQuery { Id = _coachingProgram.Id };

            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                query.Id, default
            )).ReturnsAsync(_coachingProgram);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.BeginDate.Should().Be(_coachingProgram.BeginDate);
        }

        [Fact]
        public async Task Handle_GetCoachingProgram_EndDateShouldBeSet()
        {
            // Arrange
            var query = new GetCoachingProgramEditQuery { Id = _coachingProgram.Id };

            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                query.Id, default
            )).ReturnsAsync(_coachingProgram);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.EndDate.Should().Be(_coachingProgram.EndDate);
        }

        [Fact]
        public async Task Handle_GetCoachingProgram_BudgetAmmountShouldBeSet()
        {
            // Arrange
            var query = new GetCoachingProgramEditQuery { Id = _coachingProgram.Id };

            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                query.Id, default
            )).ReturnsAsync(_coachingProgram);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.BudgetAmmount.Should().Be(_coachingProgram.BudgetAmmount);
        }

        [Fact]
        public async Task Handle_GetCoachingProgram_HourlyRateShouldBeSet()
        {
            // Arrange
            var query = new GetCoachingProgramEditQuery { Id = _coachingProgram.Id };

            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                query.Id, default
            )).ReturnsAsync(_coachingProgram);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.HourlyRate.Should().Be(_coachingProgram.HourlyRate);
        }

        [Fact]
        public async Task Handle_GetCoachingProgram_RemainingHoursShouldBeSet()
        {
            // Arrange
            var query = new GetCoachingProgramEditQuery { Id = _coachingProgram.Id };

            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                query.Id, default
            )).ReturnsAsync(_coachingProgram);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.RemainingHours.Should().Be(_coachingProgram.RemainingHours);
        }
    }
}
