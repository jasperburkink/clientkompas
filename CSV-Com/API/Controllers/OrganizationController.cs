using Application.Organizations.Commands.CreateOrganization;
using Application.Organizations.Commands.DeleteOrganization;
using Application.Organizations.Commands.UpdateOrganization;
using Application.Organizations.Dtos;
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
            var organization = await Mediator.Send(new GetOrganizationQuery { OrganizationId = id });
            return Ok(organization);
        }

        [HttpPut]
        public async Task<ActionResult<OrganizationDto>> Put(UpdateOrganizationCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<SearchOrganizationDto>>> SearchOrganizations([FromQuery] SearchOrganizationQuery query)
        {
            var organization = await Mediator.Send(query);
            return Ok(organization);
        }

        [HttpPost]
        public async Task<ActionResult<OrganizationDto>> Create(CreateOrganizationCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(DeleteOrganizationCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
    }
}
