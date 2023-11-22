using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Enums;

namespace Application.DriversLicence.Queries
{
    public class DriversLicenceDto : IMapFrom<DriversLicenceEnum>
    {
        public string DriverLicenceCategory { get; set; }

        public int DriverLicenceId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DriversLicenceEnum, DriversLicenceDto>()
                .ForMember(dlDto => dlDto.DriverLicenceCategory, c => c.MapFrom(dlEnum => Enum.GetName(typeof(DriversLicenceEnum), dlEnum)))
                .ForMember(dlDto => dlDto.DriverLicenceId, c => c.MapFrom(dlEnum => (int)dlEnum));
        }
    }
}
