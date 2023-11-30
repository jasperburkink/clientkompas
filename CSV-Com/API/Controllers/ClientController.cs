using Application.Clients.Commands.AddClientDriversLicence;
using Application.Clients.Commands.CreateClient;
using Application.Clients.Commands.DeleteClientDriversLicence;
using Application.Clients.Queries.GetClients;
using Application.Clients.Queries.SearchClients;
using Domain.CVS.Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // [EnableCors(origins: "localhost:3000", headers: "*", methods: "*")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<ClientDto>> Get([FromQuery] GetClientsQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> Get(int id)
        {
            try
            {
                var client = await Mediator.Send(new GetClientQuery { ClientId = id });
                return Ok(client);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ClientDto>> Create(CreateClientCommand command)
        {
            return await Mediator.Send(command);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<int>> AddDriversLicence(AddClientDriversLicenceCommand command)
        {
            return await Mediator.Send(command);
        }

        //TODO: implement with new Mediator structure
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Client value)
        {
            throw new NotImplementedException();
        }

        //TODO: implement with new Mediator structure
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        [Route("[action]")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> DeleteDriversLicence(DeleteClientDriversLicenceCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }


        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<SearchClientDto>>> SearchClients([FromQuery] SearchClientsQuery query)
        {
            try
            {
                var clients = await Mediator.Send(query);
                return Ok(clients);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
