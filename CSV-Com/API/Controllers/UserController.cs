using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUnitOfWork unitOfWork) : ControllerBase
    {
        [HttpGet]
        public IEnumerable<User> Get()
        {
            var users = unitOfWork.UserRepository.Get().ToList();

            return users;
        }

        [HttpGet("{id}")]
        public User Get(int id)
        {
            var user = unitOfWork.UserRepository.Get(u => u.Id.Equals(id)).First();
            return user;
        }

        [HttpPost]
        public void Post([FromBody] User user)
        {
            unitOfWork.UserRepository.Insert(user);
            unitOfWork.Save();
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] User value)
        {
            unitOfWork.UserRepository.Update(value);
            unitOfWork.Save();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            unitOfWork.UserRepository.Delete(id);
            unitOfWork.Save();
        }
    }
}
