using Application.CoachingPrograms.Queries.GetCoachingProgram;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using AutoMapper;
using Moq;
using TestData;
using TestData.CoachingProgram;

namespace Application.UnitTests.CoachingPrograms.Queries.GetCoachingProgram
{
    public class GetCoachingProgramQueryTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetCoachingProgramQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        private readonly Domain.CVS.Domain.CoachingProgram _coachingProgram;

        public GetCoachingProgramQueryTests()
        {
            _unitOfWorkMock = new();
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _handler = new GetCoachingProgramQueryHandler(_unitOfWorkMock.Object, _mapper);

            ITestDataGenerator<Domain.CVS.Domain.CoachingProgram> coachingProgramDataGenerator = new CoachingProgramDataGenerator(true);
            _coachingProgram = coachingProgramDataGenerator.Create();
        }

        [Fact]
        public async Task Handle_GetCoachingProgram_ShouldReturnCoachingProgramDto()
        {
            // Arrange
            var query = new GetCoachingProgramQuery { Id = _coachingProgram.Id };

            var coachingProgramDto = new GetCoachingProgramDto
            {
                Id = _coachingProgram.Id,
                Title = _coachingProgram.Title,
                BeginDate = _coachingProgram.BeginDate,
                EndDate = _coachingProgram.EndDate,
                ClientFullName = _coachingProgram.Client.FullName,
                CoachingProgramType = _coachingProgram.CoachingProgramType,
                HourlyRate = _coachingProgram.HourlyRate,
                RemainingHours = _coachingProgram.RemainingHours,
                OrderNumber = _coachingProgram.OrderNumber,
                BudgetAmmount = _coachingProgram.BudgetAmmount,
                OrganizationName = _coachingProgram.Organization.OrganizationName
            };

            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                query.Id, It.IsAny<string>(), default
            )).ReturnsAsync(_coachingProgram);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(coachingProgramDto);
        }

        [Fact]
        public void Handle_CoachingProgram_ThrowsNotFoundException()
        {
            // Arrange
            var query = new GetCoachingProgramQuery { Id = 0 };
            Domain.CVS.Domain.CoachingProgram coachingProgram = null;
            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetByIDAsync(
                query.Id, It.IsAny<string>(), default
            )).ReturnsAsync(coachingProgram);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, default));
        }
    }
}
