using Application.BenefitForms.Commands;
using Application.BenefitForms.Commands.CreateBenefitForm;
using Application.BenefitForms.Queries;
using Application.BenefitForms.Queries.GetBenefitForm;
using Application.Common.Interfaces.CVS;
using Domain.CVS.Domain;
using Microsoft.AspNetCore.Mvc;
using Application.BenefitForms.Commands.DeleteBenefitForm;
using Application.BenefitForms.Commands.UpdateBenefitForm;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Application.Common.Exceptions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BenefitFormController : ApiControllerBase
        {
            [HttpPost]
            public async Task<ActionResult<BenefitForm>> Create(CreateBenefitFormCommand command)
            {
                try
                {
                    var result = await Mediator.Send(command);
                    return Ok(new { id = result.Id, name = result.Name });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex);
                }
            }

            [HttpGet]
            public async Task<IEnumerable<BenefitFormDto>> Get([FromQuery] GetBenefitFormQuery query)
            {
                return await Mediator.Send(query);
            }

            //TODO: implement with new Mediator structure
            [HttpPut]
            public async Task<ActionResult<BenefitForm>> Put(UpdateBenefitFormCommand command)
            {
                try
                {
                    var result = await Mediator.Send(command);
                    return Ok(new { id = result.Id, name = result.Name });
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

            //TODO: implement with new Mediator structure
            [HttpDelete]
            public async Task<ActionResult<BenefitForm>> Delete(DeleteBenefitFormCommand command)
            {
                try
                {
                    var result = await Mediator.Send(command);
                    return Ok(new { id = result.Id, name = result.Name });
                }
               /* catch (DomainObjectInUseExeption ex)
                {
                    return StatusCode(409, ex);
                }*/
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




