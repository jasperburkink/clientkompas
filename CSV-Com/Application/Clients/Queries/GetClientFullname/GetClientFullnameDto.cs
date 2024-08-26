using Application.Common.Mappings;
using AutoMapper;
using Domain.CVS.Domain;

namespace Application.Clients.Queries.GetClientFullname
{
    public class GetClientFullnameDto : IMapFrom<Client>
    {
        public required int Id { get; set; }

        public required string ClientFullname { get; set; }

        public void Mapping(Profile profile)
        {
            // TODO: Get the right text value for the enum values. Depends on language user.
            profile.CreateMap<Client, GetClientFullnameDto>()
                .ForMember(cDto => cDto.ClientFullname, s => s.MapFrom(c => c.FullName));
        }
    }
}
