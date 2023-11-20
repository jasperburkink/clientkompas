using Application.MaritalStatuses.Commands.CreateMaritalStatus;
using Application.MaritalStatuses.Queries.GetMaritalStatus;
using Microsoft.AspNetCore.Mvc;
using Application.MaritalStatuses.Commands.DeleteMaritalStatus;
using Application.MaritalStatuses.Commands.UpdateMaritalStatus;
using Application.Common.Exceptions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaritalStatusController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<MaritalStatusDto>> Create(CreateMaritalStatusCommand command)
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MaritalStatusDto>>> Get([FromQuery] GetMaritalStatusQuery query)
        {
            try
            {
                var result = await Mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        //TODO: implement with new Mediator structure
        [HttpPut]
        public async Task<ActionResult<MaritalStatusDto>> Put(UpdateMaritalStatusCommand command)
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
        [HttpDelete]
        public async Task<ActionResult> Delete(DeleteMaritalStatusCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return Ok();
            }
            catch (DomainObjectInUseExeption ex)
            {
                return StatusCode(409, ex);
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
