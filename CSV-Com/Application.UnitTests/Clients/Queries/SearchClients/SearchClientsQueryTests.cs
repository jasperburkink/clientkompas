using Application.Clients.Queries.SearchClients;
using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using Moq;
using TestData;
using TestData.Client;

namespace Application.UnitTests.Clients.Queries.SearchClients
{
    public class SearchClientsQueryTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly SearchClientsQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;
        private readonly Client _client1, _client2, _client3;

        public SearchClientsQueryTests()
        {
            _unitOfWorkMock = new();
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _handler = new SearchClientsQueryHandler(_unitOfWorkMock.Object, _mapper);

            ITestDataGenerator<Client> clientDataGenerator = new ClientDataGenerator(false);
            _client1 = clientDataGenerator.Create();
            _client2 = clientDataGenerator.Create();
            _client3 = clientDataGenerator.Create();
        }

        [Fact]
        public async Task Handle_SearchClients_ShouldReturnSearchClientDtos()
        {
            // Arrange
            var query = new SearchClientsQuery { SearchTerm = _client1.LastName };

            var clients = new List<Client>
            {
                _client1,
                _client2
            }.AsQueryable();

            var clientDtos = clients.Select(c => new SearchClientDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName,
                Initials = c.Initials,
                PrefixLastName = c.PrefixLastName,
                Id = c.Id
            }).ToList();

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.FullTextSearch(
                query.SearchTerm, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Client, object>>>()
            )).ReturnsAsync([.. clients]);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(clientDtos);
        }

        [Fact]
        public async Task Handle_NoMatchingClients_ShouldReturnEmptyList()
        {
            // Arrange
            var query = new SearchClientsQuery { SearchTerm = "NonExisting" };

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.FullTextSearch(
                query.SearchTerm,
                It.IsAny<CancellationToken>(),
                It.IsAny<System.Linq.Expressions.Expression<Func<Client, object>>>()
            )).ReturnsAsync([]);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_SearchClients_ShouldReturnSortedDtos()
        {
            // Arrange
            var query = new SearchClientsQuery { SearchTerm = "e" };

            _client1.FirstName = "Jane";
            _client1.LastName = "Doe";
            _client1.FullName = "Jane Doe";
            _client2.FirstName = "John";
            _client2.LastName = "Doe";
            _client2.FullName = "John Doe";
            _client3.FirstName = "Alice";
            _client3.LastName = "Smith";
            _client3.FullName = "Alice Smith";

            var clients = new List<Client>
            {
                _client3,
                _client2,
                _client1
            }.AsQueryable();

            var clientDtos = clients
                .Where(c => c.FirstName.Contains(query.SearchTerm) || c.LastName.Contains(query.SearchTerm))
                .Select(c => new SearchClientDto
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Initials = c.Initials,
                    PrefixLastName = c.PrefixLastName,
                    Id = c.Id
                })
                .OrderBy(sc => sc.LastName)
                .ThenBy(sc => sc.FirstName)
                .ToList();

            _unitOfWorkMock.Setup(uw => uw.ClientRepository.FullTextSearch(
                query.SearchTerm, It.IsAny<CancellationToken>(), It.IsAny<System.Linq.Expressions.Expression<Func<Client, object>>>()
            )).ReturnsAsync([.. clients]);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(clientDtos);
            result.First().FirstName.Should().Be(_client1.FirstName);
            result.Last().FirstName.Should().Be(_client3.FirstName);
        }
    }
}
