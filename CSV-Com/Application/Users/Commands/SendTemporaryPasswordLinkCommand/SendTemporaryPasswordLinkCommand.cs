using Application.Common.Models;
using Application.Common.Security;
using Domain.Authentication.Constants;

namespace Application.Users.Commands.SendTemporaryPasswordLinkCommand
{
    [Authorize(Policy = Policies.UserManagement)]
    public record SendTemporaryPasswordLinkCommand : IRequest<Result<SendTemporaryPasswordLinkCommandDto>>
    {
        public string EmailAddress { get; set; }
    }
}
