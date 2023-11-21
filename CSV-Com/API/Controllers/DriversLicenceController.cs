using Application.DriversLicence.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversLicenceController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<DriversLicenceDto>> Get([FromQuery] GetDriversLicenceQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
