using Application.Organizations.Queries.GetOrganizations;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<GetOrganizationDto>> Get([FromQuery] GetOrganizationsQuery query)
        {
            return await Mediator.Send(query);
        }
    }
}
