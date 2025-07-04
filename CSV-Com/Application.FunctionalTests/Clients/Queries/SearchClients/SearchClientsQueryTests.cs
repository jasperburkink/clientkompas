﻿using Application.Clients.Queries.SearchClients;
using Domain.Authentication.Constants;
using Domain.CVS.Domain;
using TestData;
using TestData.Client;

namespace Application.FunctionalTests.Clients.Queries.SearchClients
{
    public class SearchClientsQueryTests : BaseTestFixture
    {
        private Client _client;
        private SearchClientDto _searchClientDto;

        [SetUp()]
        public async Task Initialize()
        {
            // Arrange
            ITestDataGenerator<Client> testDataGeneratorClient = new ClientDataGenerator();

            _client = testDataGeneratorClient.Create();

            await AddAsync(_client);

            _searchClientDto = new SearchClientDto
            {
                FirstName = _client.FirstName,
                LastName = _client.LastName,
                Id = _client.Id,
                Initials = _client.Initials,
                PrefixLastName = _client.PrefixLastName
            };
        }

        [Test]
        public async Task Handle_ClientIsNotDeactivated_ReturnClient()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new SearchClientsQuery { SearchTerm = _client.LastName };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().Contain(c => c.Id == _client.Id);
        }

        [Test]
        public async Task Handle_ClientIsAlreadyDeactivated_DoesNotReturnClient()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var client = await FindAsync<Client>(_client.Id);
            if (client is null)
            {
                Assert.Fail("Error while setting up deactivation datetime because client cannot be found.");
            }
            else
            {
                client.SetPrivate(c => c.DeactivationDateTime, new DateTime(2020, 04, 01));
                await UpdateAsync(client);
            }

            var query = new SearchClientsQuery { SearchTerm = _client.LastName };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().NotContain(c => c.Id == _client.Id, because: "Client is already deactivated and should not be shown in the searchresults.");
        }

        [Test]
        public async Task Handle_SearchClientByRightFirstName_ReturnsClients()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new SearchClientsQuery { SearchTerm = _client.FirstName };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().HaveCount(1);
            clients.First().Should().BeEquivalentTo(_searchClientDto);
        }

        [Test]
        public async Task Handle_SearchClientByWrongFirstName_ReturnsNoClients()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new SearchClientsQuery { SearchTerm = $"Wrong{_client.FirstName}" };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().BeEmpty();
        }

        [Test]
        public async Task Handle_SearchClientByRightLastName_ReturnsClients()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new SearchClientsQuery { SearchTerm = _client.LastName };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().HaveCount(1);
            clients.First().Should().BeEquivalentTo(_searchClientDto);
        }

        [Test]
        public async Task Handle_SearchClientByWrongLastName_ReturnsNoClients()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new SearchClientsQuery { SearchTerm = $"Wrong{_client.LastName}" };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().BeEmpty();
        }

        [Test]
        public async Task Handle_SearchClientByRightPrefixLastName_ReturnsClients()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new SearchClientsQuery { SearchTerm = _client.PrefixLastName };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().HaveCount(1);
            clients.First().Should().BeEquivalentTo(_searchClientDto);
        }

        [Test]
        public async Task Handle_SearchClientByWrongPrefixLastName_ReturnsNoClients()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new SearchClientsQuery { SearchTerm = $"Wrong{_client.PrefixLastName?.Replace(' ', '_')}Wrong" };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().BeEmpty();
        }

        [Test]
        public async Task Handle_SearchClientWithEmptyPrefixLastName_ReturnsNoClients()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var prefixLastName = string.Empty;
            await ResetState();
            _client.PrefixLastName = prefixLastName;
            await AddAsync(_client);

            var query = new SearchClientsQuery { SearchTerm = "PrefixLastName" };

            await RunAsAsync(Roles.Administrator);

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().BeEmpty();
        }

        [Test]
        public async Task Handle_SearchClientByFullName_ReturnsClient()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var query = new SearchClientsQuery { SearchTerm = _client.FullName };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().HaveCount(1);
            clients.First().Should().BeEquivalentTo(_searchClientDto);
        }

        [Test]
        public async Task Handle_SearchClientByPartionOfFullNameMiddle_ReturnsClient()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var prefixLastName = "van";
            await ResetState();
            _client.PrefixLastName = prefixLastName;
            _searchClientDto.PrefixLastName = prefixLastName;
            await AddAsync(_client);

            var partionFullname = $"{_client.FirstName[(_client.FirstName.Length - 1)..]} {_client.PrefixLastName[..(_client.PrefixLastName.Length - 1)]}";
            var query = new SearchClientsQuery { SearchTerm = partionFullname };

            await RunAsAsync(Roles.Administrator);

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().HaveCount(1);
            clients.First().Should().BeEquivalentTo(_searchClientDto);
        }

        [Test]
        public async Task Handle_SearchClientByPartionOfFullNameEndAndBeginning_ReturnsClient()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var partionFullname = $"{_client.FirstName[..(_client.FirstName.Length - 1)]} {_client.LastName[(_client.LastName.Length - 1)..]}";
            var query = new SearchClientsQuery { SearchTerm = partionFullname };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().HaveCount(1);
            clients.First().Should().BeEquivalentTo(_searchClientDto); ;
        }

        [Test]
        public async Task Handle_SearchTermIsEmpty_ReturnsAllClients()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var searchTerm = string.Empty;
            var query = new SearchClientsQuery { SearchTerm = searchTerm };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().HaveCount(1);
            clients.First().Should().BeEquivalentTo(_searchClientDto);
        }

        [Test]
        public async Task Handle_SearchTermIsNull_ReturnsAllClients()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            string searchTerm = null;
            var query = new SearchClientsQuery { SearchTerm = searchTerm };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().HaveCount(1);
            clients.First().Should().BeEquivalentTo(_searchClientDto);
        }

        [Test]
        public async Task Handle_SearchByPartOfFirstName_ReturnsClient()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var searchTerm = $"{_client.FirstName[..(_client.FirstName.Length - 1)]}";
            var query = new SearchClientsQuery { SearchTerm = searchTerm };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().HaveCount(1);
            clients.First().Should().BeEquivalentTo(_searchClientDto);
        }

        [Test]
        public async Task Handle_SearchByOneRightLetter_ReturnsClient()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var searchTerm = _client.FirstName.First().ToString();
            var query = new SearchClientsQuery { SearchTerm = searchTerm };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().HaveCount(1);
            clients.First().Should().BeEquivalentTo(_searchClientDto);
        }

        [Test]
        public async Task Handle_SearchByOneWrongLetter_ReturnsNoClient()
        {
            // Arrange
            await RunAsAsync(Roles.Administrator);

            var existingClients = await GetAsync<Client>();
            var searchTerm = GetLetterNotInClientNames(existingClients);
            var query = new SearchClientsQuery { SearchTerm = searchTerm };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().BeEmpty();
        }

        [TestCase(Roles.SystemOwner)]
        [TestCase(Roles.Licensee)]
        [TestCase(Roles.Administrator)]
        [TestCase(Roles.Coach)]
        public async Task Handle_RunAsRole_ReturnClient(string role)
        {
            // Arrange
            await RunAsAsync(role);

            var query = new SearchClientsQuery { SearchTerm = _client.LastName };

            // Act
            var clients = await SendAsync(query);

            // Assert
            clients.Should().Contain(c => c.Id == _client.Id);
        }

        // TODO: Move this method to a utils class in a test library
        private string GetLetterNotInClientNames(ICollection<Client> clients)
        {
            const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var allNames = string.Concat(clients.Select(c => c.FullName.ToUpper()));
            var missingLetters = Alphabet.Except(allNames).ToList();

            return missingLetters.FirstOrDefault().ToString();
        }
    }
}
