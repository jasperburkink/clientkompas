using Application.Clients.Queries.GetClients;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.ValueObjects;
using FluentAssertions;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Queries.GetClients
{
    public class GetClientsQueryTests
    {

        [Test]
        public async Task Handle_CorrectFlow_ShouldReturnClients()
        {
            // Arrange
            await RunAsDefaultUserAsync();
            var client = new Client
            {
                FirstName = "Jan",
                LastName = "Jansen",
                Address = Address.From("Dorpstraat", 1, string.Empty, "1234AB", "Amsterdam"),
                TelephoneNumber = "0123456789",
                Gender = Gender.Men,
                EmailAddress = "a@b.com",
                Initials = "J.W.C.",
                Remarks = "Jan is geweldig",
                DateOfBirth = new DateOnly(1960, 12, 25),
                MaritalStatus = new MaritalStatus
                {
                    Name = "Gehuwd"
                }
            };
            await AddAsync(client);

            var query = new GetClientsQuery();

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull().And.HaveCount(1);
            result.First().Id.Should().Be(client.Id);
        }

        [Test]
        public async Task Handle_UserIsAnomymousUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var query = new GetClientsQuery();

            // Act
            var result = () => SendAsync(query);

            // Assert
            await result.Should().ThrowAsync<UnauthorizedAccessException>();
        }
    }
}
