using Application.CoachingPrograms.Commands.CreateCoachingProgram;
using Application.CoachingPrograms.Commands.UpdateCoachingProgram;
using Application.CoachingPrograms.Queries.GetCoachingProgram;
using Application.CoachingPrograms.Queries.GetCoachingProgramsByClient;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoachingProgramController : ApiControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCoachingProgramDto>> Get(int id)
        {
            try
            {
                var client = await Mediator.Send(new GetCoachingProgramQuery { Id = id });
                return Ok(client);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<GetCoachingProgramsByClientDto>> GetCoachingProgramsByClient(int id)
        {
            try
            {
                var client = await Mediator.Send(new GetCoachingProgramsByClientQuery { ClientId = id });
                return Ok(client);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CreateCoachingProgramCommandDto>> Create(CreateCoachingProgramCommand command)
        {
            try
            {
                return await Mediator.Send(command);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPut]
        public async Task<ActionResult<UpdateCoachingProgramCommandDto>> Update(UpdateCoachingProgramCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(ex.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
