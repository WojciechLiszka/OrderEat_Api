using FastFood.Application.SpecialDiet.Commands;
using FastFood.Application.SpecialDiet.Commands.CreateSpecialDiet;
using FastFood.Application.SpecialDiet.Commands.DeleteSpecialDiet;
using FastFood.Application.SpecialDiet.Commands.UpdateSpecialDiet;
using FastFood.Application.SpecialDiet.Queries;
using FastFood.Application.SpecialDiet.Queries.GetSpecialDietById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("api/specialDiet")]
    public class SpecialDietController : Controller
    {
        private readonly IMediator _mediator;

        public SpecialDietController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([FromQuery] CreateSpecialDietCommand command)
        {
            var response = await _mediator.Send(command);
            return Created($"api/specialDiet/{response}", null);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromQuery] DietDto dto)
        {
            var request = new UpdateSpecialDietCommand()
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
            };

            await _mediator.Send(request);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var request = new DeleteSpecialDietCommand()
            {
                Id = id
            };
            await _mediator.Send(request);
            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetDietDto>> GetById([FromRoute] int id)
        {
            var request = new GetSpecialDietByIdQuery()
            {
                Id = id
            };

            var result = await _mediator.Send(request);

            return Ok(result);
        }
    }
}