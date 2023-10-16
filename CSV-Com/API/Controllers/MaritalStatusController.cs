using Application.MaritalStatuses.Commands;
using Application.MaritalStatuses.Commands.CreateMaritalStatus;
using Application.MaritalStatuses.Queries;
using Application.MaritalStatuses.Queries.GetMaritalStatus;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Microsoft.AspNetCore.Mvc;
using Application.MaritalStatuses.Commands.DeleteMaritalStatus;
using Application.MaritalStatuses.Commands.UpdateMaritalStatus;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Application.Common.Exceptions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaritalStatusController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<MaritalStatus>> Create(CreateMaritalStatusCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(new { id = result.Id, name = result.Name, created = result.Created });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        public async Task<IEnumerable<MaritalStatusDto>> Get([FromQuery] GetMaritalStatusQuery query)
        {
            return await Mediator.Send(query);
        }

        //TODO: implement with new Mediator structure
        [HttpPut]
        public async Task<ActionResult<MaritalStatus>> Put(UpdateMaritalStatusCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(new { id = result.Id, name = result.Name, created = result.Created});
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
        public async Task<ActionResult<MaritalStatus>> Delete(DeleteMaritalStatusCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(new { id = result.Id, name = result.Name, created = result.Created });
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
