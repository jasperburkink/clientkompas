using Application.Common.Interfaces.Authentication;
using Application.Common.Security;

namespace Application.Menu.Queries.GetMenuByUser
{
    [Authorize]
    public record GetMenuByUserQuery : IRequest<GetMenuByUserDto>
    {
        public string UserId { get; set; } = null!;
    }

    public class GetMenuByUserHandler : IRequestHandler<GetMenuByUserQuery, GetMenuByUserDto>
    {
        private readonly IIdentityService _identityService;
        private readonly IMenuService _menuService;

        public GetMenuByUserHandler(IIdentityService identityService, IMenuService menuService)
        {
            _identityService = identityService;
            _menuService = menuService;
        }

        public async Task<GetMenuByUserDto> Handle(GetMenuByUserQuery request, CancellationToken cancellationToken)
        {
            var roles = await _identityService.GetRolesAsync(request.UserId);

            if (!roles.Any())
            {
                throw new NotFoundException($"No roles found for user '{request.UserId}'.", request.UserId);
            }

            var role = roles.First();

            var menuItems = _menuService.GetMenuByRole(role);

            return new GetMenuByUserDto
            {
                Role = role,
                MenuItems = menuItems
            };
        }
    }
}
