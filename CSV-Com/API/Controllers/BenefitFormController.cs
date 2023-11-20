using Application.BenefitForms.Commands.CreateBenefitForm;
using Application.BenefitForms.Queries.GetBenefitForm;
using Microsoft.AspNetCore.Mvc;
using Application.BenefitForms.Commands.DeleteBenefitForm;
using Application.BenefitForms.Commands.UpdateBenefitForm;
using Application.Common.Exceptions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BenefitFormController : ApiControllerBase
        {
            [HttpPost]
            public async Task<ActionResult<BenefitFormDto>> Create(CreateBenefitFormCommand command)
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
            public async Task<ActionResult<IEnumerable<BenefitFormDto>>> Get([FromQuery] GetBenefitFormQuery query)
            {
                try
                {
                    return Ok(await Mediator.Send(query));
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex);
                }
            }

            [HttpPut]
            public async Task<ActionResult<BenefitFormDto>> Put(UpdateBenefitFormCommand command)
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
            public async Task<ActionResult> Delete(DeleteBenefitFormCommand command)
            {
                try
                {
                    await Mediator.Send(command);
                    return Ok();
                }
                catch (DomainObjectInUseExeption ex)
                {
                    return StatusCode(400, ex);
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




