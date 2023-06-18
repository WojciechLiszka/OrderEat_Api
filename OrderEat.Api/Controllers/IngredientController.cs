using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderEat.Application.Ingredient.Command;
using OrderEat.Application.Ingredient.Command.CreateIngredient;
using OrderEat.Application.Ingredient.Command.DeleteIngredient;
using OrderEat.Application.Ingredient.Command.UpdateIngredient;
using OrderEat.Application.Ingredient.Queries;
using OrderEat.Application.Ingredient.Queries.GetIngredientById;
using OrderEat.Application.Ingredient.Queries.GetIngredientsFromDish;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderEat.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class IngredientController : Controller
    {
        private readonly IMediator _mediator;

        public IngredientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("dish/{dishId}/ingredient")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Add ingredient to dish")]
        public async Task<ActionResult<string>> Create([FromRoute] int dishId, [FromBody] IngredientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var request = new CreateIngredientCommand()
            {
                DishId = dishId,
                Name = dto.Name,
                Description = dto.Description,
                Prize = dto.Prize,
                IsRequired = dto.IsRequired
            };

            var id = await _mediator.Send(request);

            return Created($"api/ingredient/{id}", null);
        }

        [HttpDelete]
        [Route("ingredient/{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType(typeof(string), 204)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Delete ingredient")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var request = new DeleteIngredientCommand()
            {
                Id = id
            };

            await _mediator.Send(request);

            return NoContent();
        }

        [HttpGet]
        [Route("ingredient/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetIngredientDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Get ingredient by id")]
        public async Task<ActionResult<GetIngredientDto>> GetById([FromRoute] int id)
        {
            var request = new GetIngredientByIdQuery()
            {
                Id = id
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("dish/{id}/ingredient")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<GetIngredientDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Get ingredients from dish")]
        public async Task<ActionResult<List<GetIngredientDto>>> GetFromDish([FromRoute] int id)
        {
            var request = new GetIngredientsFromDishQuery()
            {
                DishId = id
            };

            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [HttpPut]
        [Route("ingredient")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Update ingredient")]
        public async Task<ActionResult> Update([FromBody] UpdateIngredientCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _mediator.Send(command);

            return Ok();
        }
    }
}