using FastFood.Application.Ingredient.Command;
using FastFood.Application.Ingredient.Command.CreateIngredient;
using FastFood.Application.Ingredient.Command.DeleteIngredient;
using FastFood.Application.Ingredient.Command.UpdateIngredient;
using FastFood.Application.Ingredient.Queries;
using FastFood.Application.Ingredient.Queries.GetIngredientById;
using FastFood.Application.Ingredient.Queries.GetIngredientsFromDish;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class IngredientController : Controller
    {
        private readonly IMediator _mediator;

        public IngredientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("dish/{dishId}/ingredient")]
        public async Task<ActionResult<string>> Create([FromRoute] int dishId, [FromQuery] IngredientDto dto)
        {
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

        [HttpGet]
        [Route("ingredient/{id}")]
        public async Task<ActionResult<GetIngredientDto>> GetById([FromRoute] int id)
        {
            var request = new GetIngredientByIdQuery()
            {
                Id = id
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpDelete]
        [Route("ingredient/{id}")]
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
        [Route("dish/{id}/ingredient")]
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
        public async Task<ActionResult> Update([FromQuery] UpdateIngredientCommand command)
        {
            await _mediator.Send(command);

            return Ok();
        }
    }
}