using Application.Clients.Queries.SearchClients;
using Application.Common.Interfaces.CVS;
using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using Moq;

namespace Application.UnitTests.Clients.Queries.SearchClients
{
    public class SearchClientsQueryTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly SearchClientsQueryHandler _handler;
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public SearchClientsQueryTests()
        {
            _unitOfWorkMock = new();
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<MappingProfile>());

            _mapper = _configuration.CreateMapper();
            _handler = new SearchClientsQueryHandler(_unitOfWorkMock.Object, _mapper);
        }

        [Fact]
        public async Task Handle_SearchClients_ShouldReturnSearchClientDtos()
        {
            // Arrange
            var query = new SearchClientsQuery { SearchTerm = "Doe" };

            var clients = new List<Client>
            {
                new() { FirstName = "John", LastName = "Doe", FullName = "John Doe" },
                new() { FirstName = "Jane", LastName = "Doe", FullName = "Jane Doe" }
            }.AsQueryable();

            var clientDtos = clients.Select(c => new SearchClientDto
            {
                FirstName = c.FirstName,
                LastName = c.LastName
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

            var client1 = new Client { FirstName = "Jane", LastName = "Doe", FullName = "Jane Doe" };
            var client2 = new Client { FirstName = "John", LastName = "Doe", FullName = "John Doe" };
            var client3 = new Client { FirstName = "Alice", LastName = "Smith", FullName = "Alice Smith" };

            var clients = new List<Client>
            {
                client3,
                client2,
                client1
            }.AsQueryable();

            var clientDtos = clients
                .Where(c => c.FirstName.Contains(query.SearchTerm) || c.LastName.Contains(query.SearchTerm))
                .Select(c => new SearchClientDto
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName
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
            result.First().FirstName.Should().Be(client1.FirstName);
            result.Last().FirstName.Should().Be(client3.FirstName);
        }
    }
}
