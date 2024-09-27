using Application.Authentication.Commands.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ApiControllerBase
    {
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<LoginCommandDto>> Login(LoginCommand command)
        {
            var result = await Mediator.Send(command);

            if (!result.Success || result.BearerToken == null)
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
