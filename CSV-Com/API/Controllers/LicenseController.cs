using Application.Licenses.Commands.BlockLicense;
using Application.Licenses.Commands.CreateLicense;
using Application.Licenses.Commands.UpdateLicense;
using Application.Licenses.Dtos;
using Application.Licenses.Queries;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class LicenseController : ApiControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<LicenseDto>> GetLicense(int id)
        {
            var result = await Mediator.Send(new GetLicenseQuery(id));
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<List<LicenseDto>>> GetAllLicenses()
        {
            var result = await Mediator.Send(new GetAllLicensesQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<LicenseDto>> CreateLicense(CreateLicenseCommand command)
        {
            var result = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetLicense), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<LicenseDto>> UpdateLicense(int id, UpdateLicenseCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            var result = await Mediator.Send(command);
            return Ok(result);
        }

        // Block license by id
        [HttpPost("{id}/block")]
        public async Task<IActionResult> BlockLicense(int id)
        {
            await Mediator.Send(new BlockLicenseCommand(id));
            return NoContent();
        }
    }
}
