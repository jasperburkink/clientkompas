using Application.Common.Mappings;
using Domain.CVS.Domain;

namespace Application.Users.Queries.SearchUsers
{
    public class SearchUsersQueryDto : IMapFrom<User>
    {
        public int Id { get; set; }

        public required string FirstName { get; set; }

        public string? PrefixLastName { get; set; }

        public required string LastName { get; set; }

        public required string FullName { get; set; }

        public DateTime? DeactivatedDateTime { get; set; }
    }
}
