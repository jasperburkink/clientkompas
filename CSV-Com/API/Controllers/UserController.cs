using Application.Users.Commands.CreateUser;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CreateUserCommandDto>> Create(CreateUserCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
