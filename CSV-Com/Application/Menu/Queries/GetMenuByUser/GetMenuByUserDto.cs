using Domain.CVS.Domain;

namespace Application.Menu.Queries.GetMenuByUser
{
    public class GetMenuByUserDto : IRequest<GetMenuByUserDto>
    {
        public string Role { get; set; }

        public IEnumerable<MenuItem> MenuItems { get; set; } = [];
    }
}
