using Application.Common.Mappings;
using Domain.CVS.Domain;

namespace Application.Users.Commands.CreateUser
{
    public class CreateUserCommandDto : IMapFrom<User>
    {
        public required int Id { get; set; }

        public required string FirstName { get; set; }

        public string? PrefixLastName { get; set; }

        public required string LastName { get; set; }

        public required string EmailAddress { get; set; }

        public required string TelephoneNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, CreateUserCommandDto>();
        }
    }
}
