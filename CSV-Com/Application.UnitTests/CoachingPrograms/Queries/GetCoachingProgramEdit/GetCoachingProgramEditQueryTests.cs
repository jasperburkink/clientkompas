using Application.CoachingPrograms.Queries.GetCoachingProgramEdit;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using AutoMapper;
using Moq;
using TestData;
using TestData.CoachingProgram;

namespace Application.UnitTests.CoachingPrograms.Queries.GetCoachingProgramEdit
{
    public class GetCoachingProgramEditQueryTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetCoachingProgramEditQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        private readonly Domain.CVS.Domain.CoachingProgram _coachingProgram;

        public GetCoachingProgramEditQueryTests()
        {
            _unitOfWorkMock = new();
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _handler = new GetCoachingProgramEditQueryHandler(_unitOfWorkMock.Object, _mapper);

            ITestDataGenerator<Domain.CVS.Domain.CoachingProgram> coachingProgramDataGenerator = new CoachingProgramDataGenerator(true);
            _coachingProgram = coachingProgramDataGenerator.Create();
        }

        [Fact]
        public async Task Handle_GetCoachingProgram_ShouldReturnCoachingProgramDto()
        {
            // Arrange
            var query = new GetCoachingProgramEditQuery { Id = _coachingProgram.Id };
            _coachingProgram.CoachingProgramType = Domain.CVS.Enums.CoachingProgramType.PGB;

            var coachingProgramDto = new GetCoachingProgramEditDto
            {
                Id = _coachingProgram.Id,
                Title = _coachingProgram.Title,
                BeginDate = _coachingProgram.BeginDate,
                EndDate = _coachingProgram.EndDate,
                ClientId = _coachingProgram.ClientId,
                CoachingProgramType = (int)_coachingProgram.CoachingProgramType,
                HourlyRate = _coachingProgram.HourlyRate,
                RemainingHours = _coachingProgram.RemainingHours,
                OrderNumber = _coachingProgram.OrderNumber,
                BudgetAmmount = _coachingProgram.BudgetAmmount,
                OrganizationId = _coachingProgram.OrganizationId
            };

            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                query.Id, default
            )).ReturnsAsync(_coachingProgram);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(coachingProgramDto);
        }

        [Fact]
        public async Task Handle_CoachingProgram_ThrowsNotFoundException()
        {
            // Arrange
            var query = new GetCoachingProgramEditQuery { Id = 0 };
            Domain.CVS.Domain.CoachingProgram coachingProgram = null;
            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                query.Id, default
            )).ReturnsAsync(coachingProgram);

            // Act
            var act = () => _handler.Handle(query, default);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
        }
    }
}
