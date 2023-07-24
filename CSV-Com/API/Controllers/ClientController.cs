using Application.Clients.Commands.AddClientDriversLicence;
using Application.Clients.Commands.CreateClient;
using Application.Clients.Commands.DeleteClientDriversLicence;
using Application.Clients.Queries;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<ClientDto>> Get([FromQuery] GetClientsQuery query)
        {
            return await Mediator.Send(query);
        }

        //[HttpGet("{id}")]
        //public Client Get(int id)
        //{
        //    var client = _unitOfWork.ClientRepository.Get(c => c.Id.Equals(id), includeProperties: "DriversLicences,Diagnoses,EmergencyPeople,WorkingContracts").First();
        //    return client;
        //}

        [HttpPost]
        public async Task<ActionResult<int>> Create(CreateClientCommand command)
        {
            return await Mediator.Send(command);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<int>> AddDriversLicence(AddClientDriversLicenceCommand command)
        {
            return await Mediator.Send(command);
        }

        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] Client value)
        //{
        //    _unitOfWork.ClientRepository.Update(value);
        //    _unitOfWork.Save();
        //}

        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //    _unitOfWork.ClientRepository.Delete(id);
        //    _unitOfWork.Save();
        //}

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