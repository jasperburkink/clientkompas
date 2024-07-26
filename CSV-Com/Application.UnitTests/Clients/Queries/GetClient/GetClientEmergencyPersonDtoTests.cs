using Application.Clients.Queries.GetClient;
using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using Moq;
using TestData;
using TestData.Client;

namespace Application.UnitTests.Clients.Queries.GetClient
{
    public class GetClientEmergencyPersonDtoTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetClientQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        private readonly Client _client;

        public GetClientEmergencyPersonDtoTests()
        {
            _unitOfWorkMock = new();
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _handler = new GetClientQueryHandler(_unitOfWorkMock.Object, _mapper);

            ITestDataGenerator<Client> clientDataGenerator = new ClientDataGenerator(true);
            _client = clientDataGenerator.Create();
        }

        [Fact]
        public async Task Handle_GetClient_ShouldReturnEmergencyPersonDto()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            var emergencyPeople = _client.EmergencyPeople.Select(emp => new GetClientEmergencyPersonDto
            {
                Name = emp.Name,
                TelephoneNumber = emp.TelephoneNumber
            });

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.EmergencyPeople.Should().NotBeNull().And.BeEquivalentTo(emergencyPeople);
        }
    }
}
