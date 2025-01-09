using Application.Menu.Queries.GetMenuByUser;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<GetMenuByUserDto>> GetMenuByUser([FromQuery] GetMenuByUserQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }
    }
}
