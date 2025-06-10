using System.Security.Claims;
using Application.Common.Interfaces.Authentication;

namespace API.Services
{
    public class CurrentUser(IHttpContextAccessor httpContextAccessor) : IUser
    {
        public string? CurrentUserId => httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
