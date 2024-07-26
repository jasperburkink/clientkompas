using Application.Clients.Queries.GetClientEdit;
using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using Moq;
using TestData;
using TestData.Client;

namespace Application.UnitTests.Clients.Queries.GetClientEdit
{
    public class GetClientEditDriversLicenceDtoTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetClientEditQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        private readonly Client _client;

        public GetClientEditDriversLicenceDtoTests()
        {
            _unitOfWorkMock = new();
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _handler = new GetClientEditQueryHandler(_unitOfWorkMock.Object, _mapper);

            ITestDataGenerator<Client> clientDataGenerator = new ClientDataGenerator(true);
            _client = clientDataGenerator.Create();
        }

        [Fact]
        public async Task Handle_GetClient_ShouldReturnDriverLicencesDto()
        {
            // Arrange
            var query = new GetClientEditQuery { ClientId = _client.Id };

            var driversLicences = _client.DriversLicences.Select(dl => new GetClientEditDriversLicenceDto
            {
                Id = dl.Id,
                Category = dl.Category,
                Description = dl.Description
            });

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.DriversLicences.Should().NotBeNull().And.BeEquivalentTo(driversLicences);
        }
    }
}
