using Application.Diagnoses.Commands;
using Application.Diagnoses.Commands.CreateDiagnosis;
using Application.Diagnoses.Queries;
using Application.Diagnoses.Queries.GetDiagnosis;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Microsoft.AspNetCore.Mvc;
using Application.Diagnoses.Commands.DeleteDiagnosis;
using Application.Diagnoses.Commands.UpdateDiagnosis;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Application.Common.Exceptions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Diagnosis>> Create(CreateDiagnosisCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(new { id = result.Id, name = result.Name});
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        public async Task<IEnumerable<DiagnosisDto>> Get([FromQuery] GetDiagnosisQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpPut]
        public async Task<ActionResult<Diagnosis>> Put(UpdateDiagnosisCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(new { id = result.Id, name = result.Name});
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
        public async Task<ActionResult<Diagnosis>> Delete(DeleteDiagnosisCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
                return Ok(new { id = result.Id, name = result.Name});
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
