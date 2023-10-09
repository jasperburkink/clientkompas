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

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaritalStatusController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateMaritalStatusCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        public async Task<IEnumerable<MaritalStatusDto>> Get([FromQuery] GetMaritalStatusQuery query)
        {
            return await Mediator.Send(query);
        }

        //TODO: implement with new Mediator structure
        [HttpPut]
        public async Task<ActionResult<int>> Put(UpdateMaritalStatusCommand command)
        {
            return await Mediator.Send(command);
        }

        //TODO: implement with new Mediator structure
        [HttpDelete]
        public async Task<ActionResult<int>> Delete(DeleteMaritalStatusCommand command)
        {
            return await Mediator.Send(command);
        }

    }
}
