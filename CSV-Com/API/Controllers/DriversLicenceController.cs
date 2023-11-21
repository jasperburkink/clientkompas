using Application.Common.Exceptions;
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
            try
            {
                return Ok(await Mediator.Send(query));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        [HttpPost]
        public async Task<ActionResult<DriversLicenceDto>> CreateDriversLicence(CreateDriversLicenceCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }



        [HttpPut]
        public async Task<ActionResult<DriversLicenceDto>> Put(UpdateDriversLicenceCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return StatusCode(404, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteDriversLicence(DeleteDriversLicenceCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return StatusCode(404, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
