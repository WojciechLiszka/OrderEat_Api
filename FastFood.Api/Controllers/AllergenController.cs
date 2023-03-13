using FastFood.Application.Allergen.Commands.CreateAllergen;
using FastFood.Application.Allergen.Commands.UpdateAllergen;
using FastFood.Application.Allergen.Queries.GetAllergenById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("api/allergen")]
    public class AllergenController : Controller
    {
        private readonly IMediator _mediator;

        public AllergenController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateAllergenCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<AllergenDto>> GetById([FromRoute] int id)
        {
            var result = await _mediator.Send(new GetAllergenByIdQuery()
            {
                Id = id
            });

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromQuery] AllergenDto dto)
        {
            var Request = new UpdateAllergenCommand()
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description
            };
            await _mediator.Send(Request);

            return Ok();
        }
    }
}