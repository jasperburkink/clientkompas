using Application.Licenses.Commands.BlockLicense;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using TestData;
using TestData.Organization;

namespace Application.FunctionalTests.License.Commands.BlockLicense
{
    public class BlockLicenseCommandTests : BaseTestFixture
    {
        [SetUp]
        public async Task SetUp()
        {
            await ResetState();
        }

        [Test]
        public async Task Handle_CorrectFlow_ShouldBlockLicense()
        {
            // Arrange
            var licenseHolder = new User
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                IsDeactivated = false,
                TelephoneNumber = "1234567890"

            };
            await AddAsync(licenseHolder);

            ITestDataGenerator<Organization> testDataGeneratorOrganization = new OrganizationDataGenerator();
            var organization = testDataGeneratorOrganization.Create();
            await AddAsync(organization);

            var license = new Domain.CVS.Domain.License
            {
                Status = LicenseStatus.Active,
                LicenseHolderId = licenseHolder.Id,
                OrganizationId = organization.Id
            };
            await AddAsync(license);

            var command = new BlockLicenseCommand(license.Id);

            // Act
            await SendAsync(command);

            // Assert
            var updatedLicense = await FindAsync<Domain.CVS.Domain.License>(license.Id);
            updatedLicense.Should().NotBeNull();
            updatedLicense!.Status.Should().Be(LicenseStatus.Blocked);
        }

        [Test]
        public async Task Handle_LicenseDoesNotExist_ShouldThrowNotFoundException()
        {
            // Arrange
            var command = new BlockLicenseCommand(-1); // Invalid ID

            // Act & Assert
            await FluentActions.Invoking(() => SendAsync(command))
                .Should().ThrowAsync<Common.Exceptions.NotFoundException>();
        }
    }
}
