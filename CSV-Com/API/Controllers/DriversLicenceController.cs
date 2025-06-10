using Application.DriversLicences.Commands.CreateDriversLicence;
using Application.DriversLicences.Commands.DeleteDriversLicence;
using Application.DriversLicences.Commands.UpdateDriversLicence;
using Application.DriversLicences.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversLicenceController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriversLicenceDto>>> Get([FromQuery] GetDriversLicenceQuery query)
        {
            return Ok(await Mediator.Send(query));
        }


        [HttpPost]
        public async Task<ActionResult<DriversLicenceDto>> CreateDriversLicence(CreateDriversLicenceCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }



        [HttpPut]
        public async Task<ActionResult<DriversLicenceDto>> Put(UpdateDriversLicenceCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteDriversLicence(DeleteDriversLicenceCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
    }
}
