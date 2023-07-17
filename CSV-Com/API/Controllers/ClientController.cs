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
        public async Task<IEnumerable<ClientDto>> Get(GetClientsQuery query)
        {
            return await Mediator.Send(query);
        }

        //[HttpGet("{id}")]
        //public Client Get(int id)
        //{
        //    var client = _unitOfWork.ClientRepository.Get(c => c.Id.Equals(id), includeProperties: "DriversLicences,Diagnoses,EmergencyPeople,WorkingContracts").First();
        //    return client;
        //}

        //[HttpPost]
        //public void Post([FromBody] Client client)
        //{
        //    _unitOfWork.ClientRepository.Insert(client);
        //    _unitOfWork.Save();
        //}

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
    }
}