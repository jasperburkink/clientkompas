using Application.Clients.Commands.AddClientDriversLicence;
using Application.Clients.Commands.CreateClient;
using Application.Clients.Commands.DeleteClientDriversLicence;
using Application.Clients.Commands.UpdateClient;
using Application.Clients.Dtos;
using Application.Clients.Queries.GetClients;
using Application.Common.Exceptions;
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

        [HttpPut]
        public async Task<ActionResult<ClientDto>> Put(UpdateClientCommand command)
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
    }
}