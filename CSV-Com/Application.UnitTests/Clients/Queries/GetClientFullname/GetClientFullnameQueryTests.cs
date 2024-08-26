using Application.Clients.Queries.GetClientFullname;
using Application.Common.Exceptions;
using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using Moq;
using TestData;
using TestData.Client;

namespace Application.UnitTests.Clients.Queries.GetClientFullname
{
    public class GetClientFullnameQueryTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetClientFullnameQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        private readonly Client _client;

        public GetClientFullnameQueryTests()
        {
            _unitOfWorkMock = new();
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _handler = new GetClientFullnameQueryHandler(_unitOfWorkMock.Object, _mapper);

            ITestDataGenerator<Client> clientDataGenerator = new ClientDataGenerator(false);
            _client = clientDataGenerator.Create();
        }

        [Fact]
        public async Task Handle_GetClientFullname_ShouldReturnClientFullnameDto()
        {
            // Arrange
            var query = new GetClientFullnameQuery { ClientId = _client.Id };

            var clientFullnameDto = new GetClientFullnameDto
            {
                ClientFullname = _client.FullName,
                Id = _client.Id
            };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(clientFullnameDto);
        }

        [Fact]
        public void Handle_ClientDoesNotExists_ThrowsNotFoundException()
        {
            // Arrange
            var query = new GetClientFullnameQuery { ClientId = 0 };
            Client client = null;
            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(client);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, default));
        }
    }
}
