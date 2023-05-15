using API.ViewModels;
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
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _unitOfWork;

        public GebruikersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }    

        // GET: api/<GebruikersController>
        [HttpGet]
        public IEnumerable<GebruikerViewModel> Get()
        {
            var gebruikers = _unitOfWork.GebruikerRepository.Get().ToList();            

            var gebruikersViewModel = _mapper.Map<List<GebruikerViewModel>>(gebruikers);

            return gebruikersViewModel;
        }

        // GET api/<GebruikersController>/5
        [HttpGet("{id}")]
        public GebruikerViewModel Get(int id)
        {
            var gebruiker = _unitOfWork.GebruikerRepository.Get(g => g.Id.Equals(id)).First();
            return _mapper.Map<GebruikerViewModel>(gebruiker);
        }

        // POST api/<GebruikersController>
        [HttpPost]
        public void Post([FromBody] GebruikerViewModel gebruikerViewModel)
        {
            var gebruiker = _mapper.Map<Gebruiker>(gebruikerViewModel);

            _unitOfWork.GebruikerRepository.Insert(gebruiker);
            _unitOfWork.Save();
        }

        // PUT api/<GebruikersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] GebruikerViewModel value)
        {
            var gebruiker = _mapper.Map<Gebruiker>(value);

            _unitOfWork.GebruikerRepository.Update(gebruiker);
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
