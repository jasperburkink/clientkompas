using System.Data;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Users.Queries.GetUserRoles
{
    [Authorize(Policy = Policies.UserManagement)]
    public class GetUserRolesQuery : IRequest<IEnumerable<GetUserRolesDto>>
    {
    }

    public class GetUserRolesHandler(IIdentityService identityService, IResourceMessageProvider resourceMessageProvider) : IRequestHandler<GetUserRolesQuery, IEnumerable<GetUserRolesDto>>
    {
        public async Task<IEnumerable<GetUserRolesDto>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await identityService.GetAvailableUserRolesAsync();

            if (!roles.Any())
            {
                throw new NotFoundException($"No available userroles found.");
            }

            return roles.Select(role =>
            {
                var roleName = resourceMessageProvider.GetMessage<GetUserRolesDto>(role);

                return new GetUserRolesDto
                {
                    Name = roleName,
                    Value = role
                };
            });
        }
    }
}
