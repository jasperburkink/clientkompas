using Application.Common.Interfaces.Authentication;
using Application.Common.Security;

namespace Application.Menu.Queries.GetMenuByUser
{
    [Authorize]
    public record GetMenuByUserQuery : IRequest<GetMenuByUserDto>
    {
        public string UserId { get; set; } = null!;
    }

    public class GetMenuByUserHandler(IIdentityService identityService, IMenuService menuService) : IRequestHandler<GetMenuByUserQuery, GetMenuByUserDto>
    {
        public async Task<GetMenuByUserDto> Handle(GetMenuByUserQuery request, CancellationToken cancellationToken)
        {
            var roles = await identityService.GetRolesAsync(request.UserId);

            if (!roles.Any())
            {
                throw new NotFoundException($"No roles found for user '{request.UserId}'.", request.UserId);
            }

            var role = roles.First();

            var menuItems = menuService.GetMenuByRole(role);

            return new GetMenuByUserDto
            {
                Role = role,
                MenuItems = menuItems
            };
        }
    }
}
