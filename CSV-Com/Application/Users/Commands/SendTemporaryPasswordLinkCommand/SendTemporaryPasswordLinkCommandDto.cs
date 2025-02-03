using Application.Common.Mappings;
using Domain.CVS.Domain;

namespace Application.Users.Commands.SendTemporaryPasswordLinkCommand
{
    public class SendTemporaryPasswordLinkCommandDto : IMapFrom<User>
    {
        public int UserId { get; set; }
    }
}
