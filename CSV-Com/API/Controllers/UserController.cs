using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            var users = _unitOfWork.UserRepository.Get().ToList();

            return users;
        }

        [HttpGet("{id}")]
        public User Get(int id)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.Id.Equals(id)).First();
            return user;
        }

        [HttpPost]
        public void Post([FromBody] User user)
        {
            _unitOfWork.UserRepository.Insert(user);
            _unitOfWork.Save();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User value)
        {
            _unitOfWork.UserRepository.Update(value);
            _unitOfWork.Save();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _unitOfWork.UserRepository.Delete(id);
            _unitOfWork.Save();
        }
    }
}
