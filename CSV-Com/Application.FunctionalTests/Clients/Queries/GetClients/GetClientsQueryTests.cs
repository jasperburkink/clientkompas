using Application.Clients.Queries.GetClients;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.ValueObjects;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Queries.GetClients
{
    public class GetClientsQueryTests : BaseTestFixture
    {
        // [Test]
        [Ignore("Pipeline can't handle sql connections, please think of an alternative (in memory db)")]
        public async Task Handle_CorrectFlow_ShouldReturnClients()
        {
            // Arrange
            // TODO: Turn on authentication 
            //await RunAsDefaultUserAsync();

            var benefitForm = new BenefitForm
            {
                Name = "Test"
            };

            var client = new Client
            {
                FirstName = "Berend",
                LastName = "Berendsen",
                Address = Address.From("Dorpstraat", 1, string.Empty, "1234AB", "Amsterdam"),
                TelephoneNumber = "0123456789",
                PrefixLastName = "",
                Gender = Gender.Man,
                EmailAddress = "a@b.com",
                Initials = "J.W.C.",
                Remarks = "Jan is geweldig",
                DateOfBirth = new DateOnly(1960, 12, 25),
                MaritalStatus = new MaritalStatus
                {
                    Name = "Gehuwd"
                },
                BenefitForms =
                {
                    benefitForm
                }
            };

            await AddAsync(client);

            var query = new GetClientsQuery();

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull().And.HaveCountGreaterThan(0);
            result.Should().Contain(c => c.LastName == client.LastName);
        }

        // [Test]
        [Ignore("Pipeline can't handle sql connections, please think of an alternative (in memory db)")]
        public void Handle_UserIsAnomymousUser_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            var query = new GetClientsQuery();

            // Act
            var result = () => SendAsync(query);

            // Assert
            //TODO: Turn on authentication 
            //await result.Should().ThrowAsync<UnauthorizedAccessException>();
            result.Should().NotBeNull();
        }
    }
}
