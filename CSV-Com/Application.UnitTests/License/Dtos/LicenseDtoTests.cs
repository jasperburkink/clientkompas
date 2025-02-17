using Application.Common.Mappings;
using Application.Licenses.Dtos;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;

namespace Application.UnitTests.License.Dtos
{
    public class LicenseDtoTests
    {
        private readonly IMapper _mapper;

        public LicenseDtoTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void Mapping_ConfigurationIsValid()
        {
            _mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Fact]
        public void Mapping_MapsLicenseToLicenseDto()
        {
            // Arrange
            var license = new Domain.CVS.Domain.License
            {
                Id = 1,
                CreatedAt = DateTime.Now,
                ValidUntil = DateTime.Now.AddYears(1),
                Status = LicenseStatus.Active,
                Organization = new Organization { OrganizationName = "Test Org" },
                LicenseHolder = new User
                {
                    FirstName = "John",
                    LastName = "Doe",
                    EmailAddress = "a@b.com",
                    TelephoneNumber = "1234567890"
                }
            };

            // Act
            var licenseDto = _mapper.Map<LicenseDto>(license);

            // Assert
            Assert.Equal(license.Id, licenseDto.Id);
            Assert.Equal(license.CreatedAt, licenseDto.CreatedAt);
            Assert.Equal(license.ValidUntil, licenseDto.ValidUntil);
            Assert.Equal(license.Status, licenseDto.Status);
            Assert.Equal(license.Organization.OrganizationName, licenseDto.OrganizationName);
            Assert.Equal($"{license.LicenseHolder.FirstName} {license.LicenseHolder.LastName}", licenseDto.LicenseHolderName);
        }
    }
}
