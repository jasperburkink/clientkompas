using Application.Diagnoses.Commands.CreateDiagnosis;
using Application.Diagnoses.Queries.GetDiagnosis;
using Microsoft.AspNetCore.Mvc;
using Application.Diagnoses.Commands.DeleteDiagnosis;
using Application.Diagnoses.Commands.UpdateDiagnosis;
using Application.Common.Exceptions;
using Application.Common.Models;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<DiagnosisDto>> Create(CreateDiagnosisCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiagnosisDto>>> Get([FromQuery] GetDiagnosisQuery query)
        {
            try
            {
                var result = await Mediator.Send(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut]
        public async Task<ActionResult<DiagnosisDto>> Put(UpdateDiagnosisCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return StatusCode(404, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

       
        [HttpDelete]
        public async Task<ActionResult> Delete(DeleteDiagnosisCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return Ok();
            }
            catch (NotFoundException ex)
            {
                return StatusCode(404, ex);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

    }
}
