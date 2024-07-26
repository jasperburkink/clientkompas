using Application.Clients.Queries.GetClient;
using Application.Clients.Queries.SearchClients;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using Moq;
using TestData;
using TestData.Client;

namespace Application.UnitTests.Clients.Queries.GetClient
{
    public class GetClientQueryTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetClientQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        private readonly Client _client;

        public GetClientQueryTests()
        {
            _unitOfWorkMock = new();
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _handler = new GetClientQueryHandler(_unitOfWorkMock.Object, _mapper);

            ITestDataGenerator<Client> clientDataGenerator = new ClientDataGenerator(false);
            _client = clientDataGenerator.Create();
        }

        [Fact]
        public async Task Handle_GetClient_ShouldReturnClientDto()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = _client.Id };

            var clientDto = new SearchClientDto
            {
                FirstName = _client.FirstName,
                LastName = _client.LastName,
                Initials = _client.Initials,
                PrefixLastName = _client.PrefixLastName,
                Id = _client.Id
            };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(clientDto);
        }

        [Fact]
        public void Handle_ClientDoesNotExists_ThrowsNotFoundException()
        {
            // Arrange
            var query = new GetClientQuery { ClientId = 0 };
            Client client = null;
            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(client);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, default));
        }
    }
}
