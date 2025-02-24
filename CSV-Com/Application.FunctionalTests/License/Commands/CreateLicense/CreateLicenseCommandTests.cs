using Application.Licenses.Commands.CreateLicense;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using Domain.CVS.ValueObjects;

namespace Application.FunctionalTests.License.Commands.CreateLicense
{
    public class CreateLicenseCommandTests : BaseTestFixture
    {
        [SetUp]
        public async Task SetUp()
        {
            await ResetState(); // Zorgt ervoor dat de database wordt gereset voor elke test
        }

        [Test]
        public async Task Handle_ValidCommand_ShouldCreateLicense()
        {
            // Arrange
            var licenseHolder = new User
            {
                FirstName = "Jane",
                LastName = "Smith",
                EmailAddress = "jane.smith@example.com",
                TelephoneNumber = "1234567890"
            };
            await AddAsync(licenseHolder);

            var organization = new Organization
            {
                OrganizationName = "Another Organization",
                VisitAddress = Address.From("StreetName", 456, "B", "67890", "Another City"),
                InvoiceAddress = Address.From("StreetName", 456, "B", "67890", "Another City"),
                PostAddress = Address.From("StreetName", 456, "B", "67890", "Another City"),
                ContactPersonName = "Jane Doe",
                ContactPersonFunction = "Director",
                ContactPersonTelephoneNumber = "0987654321",
                ContactPersonMobilephoneNumber = "0987654321",
                ContactPersonEmailAddress = "contact@anotherexample.com",
                PhoneNumber = "1234567890",
                Website = "https://anotherexample.com",
                EmailAddress = "contact@anotherexample.com",
                KVKNumber = "87654321",
                BTWNumber = "0987654321",
                IBANNumber = "NL91ABNA0417164301",
                BIC = "ABNANL2B"
            };
            await AddAsync(organization);

            // Verify that the objects are added correctly
            var addedLicenseHolder = await FindAsync<User>(licenseHolder.Id);
            var addedOrganization = await FindAsync<Organization>(organization.Id);
            addedLicenseHolder.Should().NotBeNull();
            addedOrganization.Should().NotBeNull();

            var command = new CreateLicenseCommand
            {
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddYears(1),
                OrganizationId = organization.Id,
                LicenseHolderId = licenseHolder.Id,
                Status = LicenseStatus.Active
            };

            // Act
            var result = await SendAsync(command);

            // Assert
            var createdLicense = await FindAsync<Domain.CVS.Domain.License>(result.Id);
            createdLicense.Should().NotBeNull();
            createdLicense.CreatedAt.Should().BeCloseTo(command.CreatedAt, TimeSpan.FromMilliseconds(1));
            createdLicense.ValidUntil.Should().BeCloseTo(command.ValidUntil, TimeSpan.FromMilliseconds(1));
            createdLicense.Status.Should().Be(command.Status);
        }

        [Test]
        public async Task Handle_OrganizationDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var licenseHolder = new User
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                TelephoneNumber = "1234567890"
            };
            await AddAsync(licenseHolder);

            var command = new CreateLicenseCommand
            {
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddYears(1),
                OrganizationId = -1, // Invalid ID
                LicenseHolderId = licenseHolder.Id,
                Status = LicenseStatus.Active
            };

            // Act & Assert
            await FluentActions.Invoking(() => SendAsync(command))
                .Should().ThrowAsync<Common.Exceptions.NotFoundException>();
        }

        [Test]
        public async Task Handle_LicenseHolderDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var organization = new Organization
            {
                OrganizationName = "Some Organization",
                VisitAddress = Address.From("StreetName", 123, "A", "12345", "City"),
                InvoiceAddress = Address.From("StreetName", 123, "A", "12345", "City"),
                PostAddress = Address.From("StreetName", 123, "A", "12345", "City"),
                ContactPersonName = "John Doe",
                ContactPersonFunction = "Manager",
                ContactPersonTelephoneNumber = "1234567890",
                ContactPersonMobilephoneNumber = "1234567890",
                ContactPersonEmailAddress = "contact@example.com",
                PhoneNumber = "0987654321",
                Website = "https://example.com",
                EmailAddress = "contact@example.com",
                KVKNumber = "12345678",
                BTWNumber = "1234567890",
                IBANNumber = "NL91ABNA0417164300",
                BIC = "ABNANL2A"
            };
            await AddAsync(organization);

            var command = new CreateLicenseCommand
            {
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddYears(1),
                OrganizationId = organization.Id,
                LicenseHolderId = -1, // Invalid ID
                Status = LicenseStatus.Active
            };

            // Act & Assert
            await FluentActions.Invoking(() => SendAsync(command))
                .Should().ThrowAsync<Common.Exceptions.NotFoundException>();
        }
    }
}
