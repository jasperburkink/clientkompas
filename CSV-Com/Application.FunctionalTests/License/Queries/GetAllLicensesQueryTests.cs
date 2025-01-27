using Application.Licenses.Queries;
using Domain.CVS.Domain;
using Domain.CVS.Enums;
using TestData;
using TestData.Organization;

namespace Application.FunctionalTests.License.Queries
{
    public class GetAllLicensesQueryTests : BaseTestFixture
    {

        [SetUp]
        public async Task SetUp()
        {
            await ResetState(); // Zorgt ervoor dat de database wordt gereset voor elke test
        }

        [Test]
        public async Task Handle_ShouldReturnAllLicenses()
        {
            // Arrange

            ITestDataGenerator<Organization> testDataGeneratorOrganization = new OrganizationDataGenerator();
            var organization = testDataGeneratorOrganization.Create();
            await AddAsync(organization);

            var licenseHolder = new User
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "John.Doe@example.com",
                IsDeactivated = false,
                TelephoneNumber = "1234567890"
            };

            await AddAsync(licenseHolder);

            var license1 = new Domain.CVS.Domain.License
            {
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddYears(1),
                OrganizationId = organization.Id,
                LicenseHolderId = licenseHolder.Id,
                Status = LicenseStatus.Active
            };
            await AddAsync(license1);

            var license2 = new Domain.CVS.Domain.License
            {
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddYears(2),
                OrganizationId = organization.Id,
                LicenseHolderId = licenseHolder.Id,
                Status = LicenseStatus.Expired
            };
            await AddAsync(license2);

            var query = new GetAllLicensesQuery();

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.Id == license1.Id);
            result.Should().Contain(x => x.Id == license2.Id);
        }

        [Test]
        public async Task Handle_NoLicenses_ShouldReturnEmptyList()
        {
            // Arrange
            var query = new GetAllLicensesQuery();

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Should().BeEmpty();
        }
    }
}
