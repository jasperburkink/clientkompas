using Application.Common.Interfaces.CVS;
using AutoMapper;
using Domain.CVS.Domain;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GebruikersController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public GebruikersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }    

        // GET: api/<GebruikersController>
        [HttpGet]
        public IEnumerable<Gebruiker> Get()
        {
            var gebruikers = _unitOfWork.GebruikerRepository.Get().ToList();            

            return gebruikers;
        }

        // GET api/<GebruikersController>/5
        [HttpGet("{id}")]
        public Gebruiker Get(int id)
        {
            var gebruiker = _unitOfWork.GebruikerRepository.Get(g => g.Id.Equals(id)).First();
            return gebruiker;
        }

        // POST api/<GebruikersController>
        [HttpPost]
        public void Post([FromBody] Gebruiker gebruiker)
        {
            _unitOfWork.GebruikerRepository.Insert(gebruiker);
            _unitOfWork.Save();
        }

        // PUT api/<GebruikersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Gebruiker value)
        {
            _unitOfWork.GebruikerRepository.Update(value);
            _unitOfWork.Save();
        }

        // DELETE api/<GebruikersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _unitOfWork.GebruikerRepository.Delete(id);
            _unitOfWork.Save();
        }
    }
}
