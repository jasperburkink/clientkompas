using Application.Licenses.Queries;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using TestData;
using TestData.Organization;

namespace Application.FunctionalTests.License.Queries
{
    public class GetLicenseQueryTests : BaseTestFixture
    {

        [Test]
        public async Task Handle_ShouldReturnLicense()
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

            var query = new GetLicenseQuery(license.Id);

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(license.Id);
            result.OrganizationName.Should().Be(organization.OrganizationName);
            result.LicenseHolderName.Should().Be($"{licenseHolder.FirstName} {licenseHolder.LastName}");
        }
    }
}
