using Application.MaritalStatuses.Commands;
using Application.MaritalStatuses.Commands.CreateMaritalStatus;
//using Application.MaritalStatuses.Queries;
//using Application.Clients.Queries.GetClients;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Microsoft.AspNetCore.Mvc;

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

        /*  [HttpGet]
          public async Task<IEnumerable<ClientDto>> Get([FromQuery] GetClientsQuery query)
          {
              return await Mediator.Send(query);
          }*/

        //TODO: implement with new Mediator structure
        /* [HttpPut("{id}")]
         public void Put(int id, [FromBody] Client value)
         {
             throw new NotImplementedException();
         }*/

        //TODO: implement with new Mediator structure
        /*  [HttpDelete("{id}")]
          public void Delete(int id)
          {
              throw new NotImplementedException();
          }*/

    }
}
