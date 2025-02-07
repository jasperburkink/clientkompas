using Application.Common.Models;
using Application.Users.Commands.CreateUser;
using Application.Users.Queries.GetUserRoles;
using Application.Users.Queries.SearchUsers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Result<CreateUserCommandDto>>> Create(CreateUserCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<GetUserRolesQuery>>> GetAvailableUserRoles([FromQuery] GetUserRolesQuery query)
        {
            var roles = await Mediator.Send(query);
            return Ok(roles);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<SearchUsersQueryDto>>> SearchUsers([FromQuery] SearchUsersQuery query)
        {
            var clients = await Mediator.Send(query);
            return Ok(clients);
        }
    }
}
