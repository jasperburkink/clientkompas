using Application.Diagnoses.Commands.CreateDiagnosis;
using Application.Diagnoses.Commands.DeleteDiagnosis;
using Application.Diagnoses.Commands.UpdateDiagnosis;
using Application.Diagnoses.Queries.GetDiagnosis;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<DiagnosisDto>> Create(CreateDiagnosisCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiagnosisDto>>> Get([FromQuery] GetDiagnosisQuery query)
        {
            var result = await Mediator.Send(query);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<DiagnosisDto>> Put(UpdateDiagnosisCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(DeleteDiagnosisCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
    }
}
