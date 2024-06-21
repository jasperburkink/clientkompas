using Application.Clients.Queries.SearchClients;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.ValueObjects;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Queries.SearchClients
{
    public class SearchClientsQueryTests
    {
        //[Test]
        [Ignore("Pipeline can't handle sql connections, please think of an alternative (in memory db)")]
        public async Task Handle_ClientIsNotDeactivated_ReturnClient()
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

            var query = new SearchClientsQuery { SearchTerm = client.LastName };

            // Act
            var clients = await SendAsync(query);

            // Assert
            Assert.IsTrue(clients.Any(c => c.Id == client.Id));
        }

        //[Test]
        [Ignore("Pipeline can't handle sql connections, please think of an alternative (in memory db)")]
        public async Task Handle_ClientIsAlreadyDeactivated_DoesNotReturnClient()
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
            client.SetPrivate(c => c.DeactivationDateTime, new DateTime(2020, 04, 01));

            await AddAsync(client);

            var query = new SearchClientsQuery { SearchTerm = client.LastName };

            // Act
            var clients = await SendAsync(query);

            // Assert
            Assert.IsFalse(clients.Any(c => c.Id == client.Id), "Client is already deactivated and should not be shown in the searchresults.");
        }
    }
}
