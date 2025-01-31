using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;
using Domain.CVS.Enums;

namespace Application.Licenses.Dtos
{
    public class LicenseDto : IMapFrom<License>
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ValidUntil { get; set; }
        public string OrganizationName { get; set; }
        public string LicenseHolderName { get; set; }
        public LicenseStatus Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<License, LicenseDto>()
                .ForMember(d => d.OrganizationName, opt => opt.MapFrom(s => s.Organization.OrganizationName))
                .ForMember(d => d.LicenseHolderName, opt => opt.MapFrom(s => s.LicenseHolder.FirstName + " " + s.LicenseHolder.LastName));
        }
    }
}
