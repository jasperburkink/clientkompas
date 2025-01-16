using Domain.CVS.Domain;
using Domain.CVS.Enums;
using TestData;
using TestData.License;

namespace Domain.UnitTests.CVS.Domain
{
    public class LicenseTests
    {
        private readonly License _license;

        public LicenseTests()
        {
            ITestDataGenerator<License> licenseDataGenerator = new LicenseDataGenerator();
            _license = licenseDataGenerator.Create();
        }

        [Fact]
        public void CreatedAt_SettingProperty_ValueHasBeenSet()
        {
            var createdAt = DateTime.UtcNow;

            _license.CreatedAt = createdAt;

            _license.CreatedAt.Should().Be(createdAt);
        }

        [Fact]
        public void ValidUntil_SettingProperty_ValueHasBeenSet()
        {
            var validUntil = DateTime.UtcNow.AddYears(1);

            _license.ValidUntil = validUntil;

            _license.ValidUntil.Should().Be(validUntil);
        }

        [Fact]
        public void Organization_SettingProperty_ValueHasBeenSet()
        {
            var organization = new Organization { OrganizationName = "New Organization" };

            _license.Organization = organization;

            _license.Organization.Should().Be(organization);
        }

        [Fact]
        public void LicenseHolder_SettingProperty_ValueHasBeenSet()
        {
            var licenseHolder = new User
            {
                FirstName = "Djomar",
                EmailAddress = "a@b.com",
                IsDeactivated = false,
                LastName = "Test",
                TelephoneNumber = "0123456789"
            };

            _license.LicenseHolder = licenseHolder;

            _license.LicenseHolder.Should().Be(licenseHolder);
        }

        [Fact]
        public void Status_SettingProperty_ValueHasBeenSet()
        {
            var status = LicenseStatus.Active;

            _license.Status = status;

            _license.Status.Should().Be(status);
        }
    }
}
