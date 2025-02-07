using Application.Common.Interfaces.CVS;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Users.Queries.SearchUsers
{
    [Authorize(Policy = Policies.ClientRead)]
    public record SearchUsersQuery : IRequest<IEnumerable<SearchUsersQueryDto>>
    {
        public string? SearchTerm { get; init; };
    }

    public class SearchUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<SearchUsersQuery, IEnumerable<SearchUsersQueryDto>>
    {
        public async Task<IEnumerable<SearchUsersQueryDto>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
        {
            return [.. (await unitOfWork.UserRepository.FullTextSearch(request.SearchTerm, cancellationToken, user => user.FullName))
                .AsQueryable()
                .ProjectTo<SearchUsersQueryDto>(mapper.ConfigurationProvider)
                .OrderBy(user => user.LastName)
                .ThenBy(user => user.FirstName)];
        }
    }
}
