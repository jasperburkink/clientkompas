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
    public class GetClientEditWorkingContractDtoTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly GetClientEditQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        private readonly Client _client;

        public GetClientEditWorkingContractDtoTests()
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
        public async Task Handle_GetClient_ShouldReturnWorkingContractsDto()
        {
            // Arrange
            var query = new GetClientEditQuery { ClientId = _client.Id };

            var workingContracts = _client.WorkingContracts.Select(dl => new GetClientEditWorkingContractDto
            {
                Id = dl.Id,
                ContractType = (int)dl.ContractType,
                Function = dl.Function,
                ToDate = dl.ToDate,
                FromDate = dl.FromDate,
                OrganizationId = dl.OrganizationId
            });

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.GetByIDAsync(
                query.ClientId, It.IsAny<string>(), default
            )).ReturnsAsync(_client);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.WorkingContracts.Should().NotBeNull().And.BeEquivalentTo(workingContracts);
        }
    }
}
