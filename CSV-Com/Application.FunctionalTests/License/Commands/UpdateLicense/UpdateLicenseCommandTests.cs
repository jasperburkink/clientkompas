using Application.Licenses.Commands.UpdateLicense;
using Application.Licenses.Dtos;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using TestData;
using TestData.Organization;

namespace Application.FunctionalTests.License.Commands.UpdateLicense
{
    public class UpdateLicenseCommandTests : BaseTestFixture
    {

        [SetUp]
        public async Task SetUp()
        {
            await ResetState(); // Zorgt ervoor dat de database wordt gereset voor elke test
        }

        [Test]
        public async Task Handle_ShouldUpdateLicense()
        {
            // Arrange
            ITestDataGenerator<Organization> testDataGeneratorOrganization = new OrganizationDataGenerator();
            var organization = testDataGeneratorOrganization.Create();
            await AddAsync(organization);

            var licenseHolder = new User
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "johndoe@email.com",
                TelephoneNumber = "1234567890"
            };
            await AddAsync(licenseHolder);

            var license = new Domain.CVS.Domain.License
            {
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddYears(1),
                OrganizationId = organization.Id,
                LicenseHolderId = licenseHolder.Id,
                Status = LicenseStatus.Active
            };
            await AddAsync(license);

            var command = new UpdateLicenseCommand
            {
                Id = license.Id,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                ValidUntil = DateTime.UtcNow.AddYears(2),
                OrganizationId = organization.Id,
                LicenseHolderId = licenseHolder.Id,
                Status = LicenseStatus.Expired
            };

            // Act
            var result = await SendAsync(command);

            // Assert
            var updatedLicense = await FindAsync<Domain.CVS.Domain.License>(result.Id);
            updatedLicense.Should().NotBeNull();
            updatedLicense.CreatedAt.Should().BeCloseTo(command.CreatedAt, TimeSpan.FromMilliseconds(1));
            updatedLicense.ValidUntil.Should().BeCloseTo(command.ValidUntil, TimeSpan.FromMilliseconds(1));
            updatedLicense.OrganizationId.Should().Be(command.OrganizationId);
            updatedLicense.LicenseHolderId.Should().Be(command.LicenseHolderId);
            updatedLicense.Status.Should().Be(command.Status);
        }

        [Test]
        public async Task Handle_LicenseDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new UpdateLicenseCommand
            {
                Id = -1, // Invalid ID
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddYears(1),
                OrganizationId = 1,
                LicenseHolderId = 1,
                Status = LicenseStatus.Active
            };

            // Act & Assert
            await FluentActions.Invoking(() => SendAsync(command))
                .Should().ThrowAsync<Common.Exceptions.NotFoundException>();
        }


        [Test]
        public async Task Handle_OrganizationDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange

            ITestDataGenerator<Organization> testDataGeneratorOrganization = new OrganizationDataGenerator();
            var organization = testDataGeneratorOrganization.Create();
            await AddAsync(organization);

            var licenseHolder = new User
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                TelephoneNumber = "1234567890"
            };
            await AddAsync(licenseHolder);

            var license = new Domain.CVS.Domain.License
            {
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddYears(1),
                OrganizationId = organization.Id,
                LicenseHolderId = licenseHolder.Id,
                Status = LicenseStatus.Active
            };
            await AddAsync(license);

            var command = new UpdateLicenseCommand
            {
                Id = license.Id,
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
            ITestDataGenerator<Organization> testDataGeneratorOrganization = new OrganizationDataGenerator();
            var organization = testDataGeneratorOrganization.Create();
            await AddAsync(organization);

            var license = new Domain.CVS.Domain.License
            {
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddYears(1),
                OrganizationId = organization.Id,
                LicenseHolder = new User
                {
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "john.doe@example.com",
                    TelephoneNumber = "1234567890"
                },
                Status = LicenseStatus.Active
            };
            await AddAsync(license);

            var command = new UpdateLicenseCommand
            {
                Id = license.Id,
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddYears(1),
                OrganizationId = organization.Id,
                LicenseHolderId = -1, // Invalid ID
                Status = LicenseStatus.Active
            };

            // Act
            Func<Task<LicenseDto>> act = async () => await SendAsync(command);

            // Assert
            await act.Should().ThrowAsync<Common.Exceptions.NotFoundException>();
        }
    }
}
