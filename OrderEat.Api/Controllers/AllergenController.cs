using OrderEat.Application.Allergen.Commands.CreateAllergen;
using OrderEat.Application.Allergen.Commands.DeleteAllergen;
using OrderEat.Application.Allergen.Commands.UpdateAllergen;
using OrderEat.Application.Allergen.Queries.GetAllergenById;
using OrderEat.Application.Allergen.Queries.GetAllergens;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderEat.Api.Controllers
{
    [ApiController]
    [Route("api/allergen")]
    [Authorize]
    public class AllergenController : Controller
    {
        private readonly IMediator _mediator;

        public AllergenController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [SwaggerOperation("Create allergen")]
        public async Task<ActionResult<string>> Create([FromBody] CreateAllergenCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var id = await _mediator.Send(command);

            return Created($"api/allergen/{id}", null);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(string), 204)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Delete allergen")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _mediator.Send(new DeleteAllergenCommand()
            {
                Id = id
            });

            return NoContent();
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<AllergenDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [SwaggerOperation("Get allergens from query")]
        public async Task<ActionResult<List<AllergenDto>>> Get([FromQuery] GetAllergensQuery query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id}")]
        [ProducesResponseType(typeof(AllergenDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Get allergen by id")]
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
        [Authorize(Roles ="Admin")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Update allergen")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateAllergenDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
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