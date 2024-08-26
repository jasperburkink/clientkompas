using System.Linq.Expressions;
using Application.CoachingPrograms.Queries.GetCoachingProgramsByClient;
using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using Moq;
using TestData;
using TestData.Client;
using TestData.CoachingProgram;

namespace Application.UnitTests.CoachingPrograms.Queries.GetCoachingProgramsByClient
{
    public class GetCoachingProgramsByClientQueryTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetCoachingProgramsByClientQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        private readonly Client _client;
        private readonly List<CoachingProgram> _coachingPrograms;

        public GetCoachingProgramsByClientQueryTests()
        {
            _unitOfWorkMock = new();
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _handler = new GetCoachingProgramsByClientQueryHandler(_unitOfWorkMock.Object, _mapper);

            ITestDataGenerator<Client> clientDataGenerator = new ClientDataGenerator(false);
            _client = clientDataGenerator.Create();

            ITestDataGenerator<CoachingProgram> coachingProgramDataGenerator = new CoachingProgramDataGenerator(true);
            _coachingPrograms = [.. coachingProgramDataGenerator.Create(5)];
        }

        [Fact]
        public async Task Handle_GetCoachingProgramsByClient_ShouldReturnCoachingProgramByClientDto()
        {
            // Arrange
            var clientId = 800;
            var numberOfClientsWithCorrectId = 2;
            var query = new GetCoachingProgramsByClientQuery { ClientId = clientId };

            for (var i = numberOfClientsWithCorrectId + 1; i < _coachingPrograms.Count(); i++)
            {
                _coachingPrograms[i].ClientId = clientId;
                _coachingPrograms[i].Client.Id = clientId;
            }

            var coachingProgramsDto = _coachingPrograms
                .Where(cp => cp.ClientId == query.ClientId)
                .Select(cp => new GetCoachingProgramsByClientDto { Id = cp.Id, Title = cp.Title });

            _unitOfWorkMock.Setup(uw => uw.CoachingProgramRepository.GetAsync(
                It.IsAny<Expression<Func<CoachingProgram, bool>>>(),
                It.IsAny<Func<IQueryable<CoachingProgram>, IOrderedQueryable<CoachingProgram>>>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(_coachingPrograms);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(numberOfClientsWithCorrectId);
            result.Should().BeEquivalentTo(coachingProgramsDto);
        }
    }
}
