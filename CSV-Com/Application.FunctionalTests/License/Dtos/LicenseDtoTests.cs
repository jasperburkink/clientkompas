using Application.Licenses.Dtos;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;

namespace Application.FunctionalTests.License.Dtos
{
    public class LicenseDtoTests : BaseTestFixture
    {
        private readonly IMapper _mapper;

        public LicenseDtoTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            _mapper = config.CreateMapper();
        }

        [Test]
        public void Should_Map_License_To_LicenseDto()
        {
            // Arrange
            var license = new Domain.CVS.Domain.License
            {
                Id = 1,
                CreatedAt = DateTime.UtcNow,
                ValidUntil = DateTime.UtcNow.AddYears(1),
                Organization = new Organization { OrganizationName = "Test Organization" },
                LicenseHolder = new User { FirstName = "John", LastName = "Doe" },
                Status = LicenseStatus.Active
            };

            // Act
            var licenseDto = _mapper.Map<LicenseDto>(license);

            // Assert
            licenseDto.Id.Should().Be(1);
            licenseDto.CreatedAt.Should().Be(license.CreatedAt);
            licenseDto.ValidUntil.Should().Be(license.ValidUntil);
            licenseDto.OrganizationName.Should().Be("Test Organization");
            licenseDto.LicenseHolderName.Should().Be("John Doe");
            licenseDto.Status.Should().Be(LicenseStatus.Active);
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Domain.CVS.Domain.License, LicenseDto>()
                .ForMember(d => d.OrganizationName, opt => opt.MapFrom(s => s.Organization.OrganizationName))
                .ForMember(d => d.LicenseHolderName, opt => opt.MapFrom(s => s.LicenseHolder.FirstName + " " + s.LicenseHolder.LastName));
        }
    }
}
