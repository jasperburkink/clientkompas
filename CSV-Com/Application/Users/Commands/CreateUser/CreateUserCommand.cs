using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Users.Commands.CreateUser
{
    [Authorize(Policy = Policies.UserManagement)]
    public record CreateUserCommand : IRequest<CreateUserCommand>
    {
        public int UserId { get; set; }


    }
}
