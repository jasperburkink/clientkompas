using Application.CoachingPrograms.Commands.CreateCoachingProgram;
using Application.CoachingPrograms.Commands.UpdateCoachingProgram;
using Application.CoachingPrograms.Queries.GetCoachingProgram;
using Application.CoachingPrograms.Queries.GetCoachingProgramEdit;
using Application.CoachingPrograms.Queries.GetCoachingProgramsByClient;
using Application.CoachingPrograms.Queries.GetCoachingProgramTypes;
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
            var coachingProgram = await Mediator.Send(new GetCoachingProgramQuery { Id = id });
            return Ok(coachingProgram);
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<GetCoachingProgramsByClientDto>> GetCoachingProgramsByClient(int id)
        {
            var coachingProgram = await Mediator.Send(new GetCoachingProgramsByClientQuery { ClientId = id });
            return Ok(coachingProgram);
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<GetCoachingProgramEditDto>> GetCoachingProgramsEdit(int id)
        {
            var coachingProgram = await Mediator.Send(new GetCoachingProgramEditQuery { Id = id });
            return Ok(coachingProgram);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<GetCoachingProgramTypesDto>> GetCoachingProgramTypes()
        {
            var coachingProgramTypes = await Mediator.Send(new GetCoachingProgramTypesQuery { });
            return Ok(coachingProgramTypes);
        }

        [HttpPost]
        public async Task<ActionResult<CreateCoachingProgramCommandDto>> Create(CreateCoachingProgramCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut]
        public async Task<ActionResult<UpdateCoachingProgramCommandDto>> Update(UpdateCoachingProgramCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
