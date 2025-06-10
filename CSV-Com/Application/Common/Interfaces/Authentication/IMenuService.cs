using Domain.CVS.Domain;

namespace Application.Common.Interfaces.Authentication
{
    public interface IMenuService
    {
        IEnumerable<MenuItem> GetMenuByRole(string role);
    }
}
