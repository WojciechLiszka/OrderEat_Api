using FastFood.Application.Ingredient.Command;
using FastFood.Application.Ingredient.Command.CreateIngredient;
using FastFood.Application.Ingredient.Command.DeleteIngredient;
using FastFood.Application.Ingredient.Command.UpdateIngredient;
using FastFood.Application.Ingredient.Queries;
using FastFood.Application.Ingredient.Queries.GetIngredientById;
using FastFood.Application.Ingredient.Queries.GetIngredientsFromDish;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Api.Controllers
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
        [Route("api/dish/{dishId}/ingredient")]
        [Authorize(Roles = "Admin,Owner")]
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
        [Route("api/ingredient/{id}")]
        [Authorize(Roles = "Admin,Owner")]
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
        [Route("api/ingredient/{id}")]
        [AllowAnonymous]
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
        [Route("api/dish/{id}/ingredient")]
        [AllowAnonymous]
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
        [Route("api/ingredient")]
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