using Application.BenefitForms.Commands.CreateBenefitForm;
using Application.BenefitForms.Commands.DeleteBenefitForm;
using Application.BenefitForms.Commands.UpdateBenefitForm;
using Application.BenefitForms.Queries.GetBenefitForm;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BenefitFormController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<BenefitFormDto>> Create(CreateBenefitFormCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BenefitFormDto>>> Get([FromQuery] GetBenefitFormQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpPut]
        public async Task<ActionResult<BenefitFormDto>> Put(UpdateBenefitFormCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(DeleteBenefitFormCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
    }
}
