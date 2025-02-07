using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Users.Queries.SearchUsers
{
    [Authorize(Policy = Policies.UserRead)]
    public record SearchUsersQuery : IRequest<IEnumerable<SearchUsersQueryDto>>
    {
        public string? SearchTerm { get; init; }
    }

    public class SearchUsersQueryHandler(IUnitOfWork unitOfWork, IIdentityService identityService, IMapper mapper) : IRequestHandler<SearchUsersQuery, IEnumerable<SearchUsersQueryDto>>
    {
        public async Task<IEnumerable<SearchUsersQueryDto>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            // TODO: Show only users that are coupled to the same licence of user that's logged in.

            // Don't show systemowners in the searchresults
            var systemOwnerIds = (await identityService.GetUsersInRolesAsync(Roles.SystemOwner))
                            .Select(systemOwner => systemOwner.CVSUserId)
                            ;

            var users = (await unitOfWork.UserRepository.FullTextSearch(
                request.SearchTerm,
                cancellationToken,
                user => user.FullName)).ToList();

            return users
                .AsQueryable()
                .Where(user => !systemOwnerIds.Contains(user.Id))
                .ProjectTo<SearchUsersQueryDto>(mapper.ConfigurationProvider)
                .OrderBy(user => user.LastName)
                .ThenBy(user => user.FirstName);
        }
    }
}
