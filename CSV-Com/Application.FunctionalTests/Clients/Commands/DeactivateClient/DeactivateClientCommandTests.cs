using Application.Clients.Commands.DeactivateClient;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.ValueObjects;
using static Application.FunctionalTests.Testing;

namespace Application.FunctionalTests.Clients.Commands.DeactivateClient
{
    public class DeactivateClientCommandTests
    {
        //[Test]
        [Ignore("Pipeline can't handle sql connections, please think of an alternative (in memory db)")]
        public async Task Handle_CorrectFlow_ShouldDeactivateClient()
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

            var command = new DeactivateClientCommand() { Id = client.Id };

            // Act
            await SendAsync(command);

            // Assert
            var deactivatedClient = await FindAsync<Client>(client.Id);

            deactivatedClient.Should().NotBeNull();
            deactivatedClient.DeactivationDateTime.Should().NotBeNull();
            deactivatedClient.DeactivationDateTime.Value.Date.Should().Be(DateTime.Now.Date);
        }

        //[Test]
        [Ignore("Pipeline can't handle sql connections, please think of an alternative (in memory db)")]
        public async Task Handle_ClientDoesNotExists_ThrowsNotFoundException()
        {
            // Arrange
            // TODO: Turn on authentication 
            //await RunAsDefaultUserAsync();

            var command = new DeactivateClientCommand() { Id = 0 };

            // Act & Assert
            Assert.ThrowsAsync<Common.Exceptions.NotFoundException>(() => SendAsync(command));
        }

        //[Test]
        [Ignore("Pipeline can't handle sql connections, please think of an alternative (in memory db)")]
        public async Task Handle_ClientIsAlreadyDeactivated_ThrowsInvalidOperationException()
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

            var command = new DeactivateClientCommand() { Id = client.Id };

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => SendAsync(command));
        }
    }
}
