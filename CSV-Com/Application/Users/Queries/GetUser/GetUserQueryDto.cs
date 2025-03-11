using Application.Common.Mappings;
using Domain.CVS.Domain;

namespace Application.Users.Queries.GetUser
{
    public class GetUserQueryDto : IMapFrom<User>
    {
        public required string FirstName { get; set; }

        public string? PrefixLastName { get; set; }

        public required string LastName { get; set; }

        public required string FullName { get; set; }

        public required string EmailAddress { get; set; }

        public required string TelephoneNumber { get; set; }

        public DateTime? DeactivationDateTime { get; set; }

        public string? CreatedByUserDescription { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, GetUserQueryDto>()
                .ForMember(userDto =>
                    userDto.CreatedByUserDescription,
                    f => f.MapFrom(user => user.CreatedByUser != null ? $"{user.CreatedByUser.FullName} ({user.CreatedByUser.EmailAddress})" : null));
        }
    }
}
