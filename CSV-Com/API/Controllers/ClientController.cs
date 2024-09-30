using Application.Clients.Commands.AddDriversLicenceToClient;
using Application.Clients.Commands.CreateClient;
using Application.Clients.Commands.DeactivateClient;
using Application.Clients.Commands.DeleteClientDriversLicence;
using Application.Clients.Commands.UpdateClient;
using Application.Clients.Dtos;
using Application.Clients.Queries.GetClient;
using Application.Clients.Queries.GetClientEdit;
using Application.Clients.Queries.GetClients;
using Application.Clients.Queries.SearchClients;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // [EnableCors(origins: "localhost:3000", headers: "*", methods: "*")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<GetClientDto>> Get([FromQuery] GetClientsQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetClientDto>> Get(int id)
        {
            var client = await Mediator.Send(new GetClientQuery { ClientId = id });
            return Ok(client);
        }

        [HttpGet("GetClientEditor/{id}")]
        public async Task<ActionResult<GetClientEditDto>> GetClientEditor(int id)
        {
            var client = await Mediator.Send(new GetClientEditQuery { ClientId = id });
            return Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult<ClientDto>> Create(CreateClientCommand command)
        {
            return await Mediator.Send(command);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<ClientDto>> AddDriversLicence(AddDriversLicenceToClientCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut]
        public async Task<ActionResult<ClientDto>> Updateclient(UpdateClientCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("DeactivateClient")]
        public async Task<IActionResult> DeactivateClient(DeactivateClientCommand command)
        {
            await Mediator.Send(command);
            return Ok();
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
            var clients = await Mediator.Send(query);
            return Ok(clients);
        }
    }
}
