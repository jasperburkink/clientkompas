using Application.Clients.Commands;
using Application.Clients.Commands.AddClientDriversLicence;
using Application.Clients.Commands.CreateClient;
using Application.Clients.Commands.DeleteClientDriversLicence;
using Application.Clients.Commands.DeleteClientDriversLicences;
using Application.Clients.Queries;
using Application.Common.Exceptions;
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
        public async Task<ActionResult<DriversLicence>> CreateDriversLicence(CreateDriversLicenceCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(new { id = result.Id, category = result.Category, description = result.Description});
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }



        [HttpPut]
        public async Task<ActionResult<DriversLicence>>  Put(UpdateDriversLicenceCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(new { id = result.Id, category = result.Category, description = result.Description});
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
        public async Task<ActionResult<DriversLicence>> DeleteDriversLicence(DeleteDriversLicenceCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(new { id = result.Id, category = result.Category, description = result.Description});
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
