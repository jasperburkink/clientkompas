using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CliëntenController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CliëntenController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<Cliënt> Get()
        {
            var cliënten = _unitOfWork.CliëntRepository.Get().ToList();

            return cliënten;
        }

        [HttpGet("{id}")]
        public Cliënt Get(int id)
        {
            var cliënt = _unitOfWork.CliëntRepository.Get(c => c.CliëntId.Equals(id)).First();
            return cliënt;
        }

        [HttpPost]
        public void Post([FromBody] Cliënt cliënt)
        {
            _unitOfWork.CliëntRepository.Insert(cliënt);
            _unitOfWork.Save();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Cliënt value)
        {
            _unitOfWork.CliëntRepository.Update(value);
            _unitOfWork.Save();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _unitOfWork.CliëntRepository.Delete(id);
            _unitOfWork.Save();
        }
    }
}