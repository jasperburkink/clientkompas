using Application.Clients.Commands;
using Application.Clients.Commands.AddClientDriversLicence;
using Application.Clients.Commands.CreateClient;
using Application.Clients.Commands.DeleteClientDriversLicence;
using Application.Clients.Commands.DeleteClientDriversLicences;
using Application.Clients.Queries;
using Application.Common.Interfaces.CVS;
using Application.DriversLicences.Commands.CreateDriversLicences;
using Application.DriversLicences.Commands.UpdateDriversLicence;
using Application.DriversLicences.Queries;
using Domain.CVS.Domain;
using Infrastructure.Persistence.CVS;
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


        [HttpPost]
        public async Task<ActionResult<int>> CreateDriversLicence(CreateDriversLicenceCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(new { Waarde = "Waarde", Waarde2 = 1 });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        [HttpPut]
        public async Task<ActionResult<int>>  Put(UpdateDriversLicenceCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok("Updated DriversLicence with an id of " + result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<ActionResult<int>> DeleteDriversLicence(DeleteDriversLicenceCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok("Deleted DriversLicence with an id of "+result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
