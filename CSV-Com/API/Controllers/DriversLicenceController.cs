using Application.Clients.Commands;
using Application.Clients.Queries;
using Application.Common.Interfaces.CVS;
using Application.DriversLicence.Queries;
using Domain.CVS.Domain;
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
