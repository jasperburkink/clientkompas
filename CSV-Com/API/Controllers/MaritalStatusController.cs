using Application.MaritalStatuses.Commands.CreateMaritalStatus;
using Application.MaritalStatuses.Commands.DeleteMaritalStatus;
using Application.MaritalStatuses.Commands.UpdateMaritalStatus;
using Application.MaritalStatuses.Queries.GetMaritalStatus;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaritalStatusController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<MaritalStatusDto>> Create(CreateMaritalStatusCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaritalStatusDto>>> Get([FromQuery] GetMaritalStatusQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        //TODO: implement with new Mediator structure
        [HttpPut]
        public async Task<ActionResult<MaritalStatusDto>> Put(UpdateMaritalStatusCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(DeleteMaritalStatusCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
    }
}
