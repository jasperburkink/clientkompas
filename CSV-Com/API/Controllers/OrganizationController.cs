using Application.Organizations.Queries.GetOrganizations;
using Application.Organizations.Queries.SearchOrganizations;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<GetOrganizationDto>> Get(int id)
        {
            try
            {
                var organization = await Mediator.Send(new GetOrganizationQuery { OrganizationId = id });
                return Ok(organization);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<SearchOrganizationDto>>> SearchOrganizations([FromQuery] SearchOrganizationQuery query)
        {
            try
            {
                var organization = await Mediator.Send(query);
                return Ok(organization);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
