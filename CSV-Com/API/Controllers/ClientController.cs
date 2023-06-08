using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Cors;

namespace API.Controllers
{
    // [EnableCors(origins: "localhost:3000", headers: "*", methods: "*")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<Client> Get()
        {
            var clienten = _unitOfWork.ClientRepository.Get().ToList();

            return clienten;
        }

        [HttpGet("{id}")]
        public Client Get(int id)
        {
            var client = _unitOfWork.ClientRepository.Get(c => c.ClientId.Equals(id)).First();
            return client;
        }

        [HttpPost]
        public void Post([FromBody] Client client)
        {
            _unitOfWork.ClientRepository.Insert(client);
            _unitOfWork.Save();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Client value)
        {
            _unitOfWork.ClientRepository.Update(value);
            _unitOfWork.Save();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _unitOfWork.ClientRepository.Delete(id);
            _unitOfWork.Save();
        }
    }
}