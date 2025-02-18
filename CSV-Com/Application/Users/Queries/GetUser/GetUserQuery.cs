using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Users.Queries.GetUser
{
    [Authorize(Policy = Policies.UserRead)]
    public class GetUserQuery :
    {
    }
}
