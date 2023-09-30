using Application.Clients.Commands;
using Application.Clients.Commands.AddClientDriversLicence;
using Application.Clients.Commands.CreateClient;
using Application.Clients.Commands.DeleteClientDriversLicence;
using Application.Clients.Commands.DeleteClientDriversLicences;
using Application.Clients.Queries;
using Application.Common.Interfaces.CVS;
using Application.DriversLicences.Commands.CreateDriversLicences;
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


        [HttpPost("CreateNewDriversLicence")]
        public async Task<ActionResult<int>> createDriversLicence(CreateDriversLicenceCommand command)
        {
            return await Mediator.Send(command);
        }



/*        [HttpPut("{id}")]
        public void Put(int id, [FromBody] DriversLicence value)
        {

            throw new NotImplementedException();
        }*/

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteDriversLicence(DeleteDriversLicenceCommand command)
        {
            return await Mediator.Send(command);
        }
    }
}
