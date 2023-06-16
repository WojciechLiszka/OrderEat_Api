using OrderEat.Application.Dish.Command.AddDietToDish;
using OrderEat.Application.SpecialDiet.Commands;
using OrderEat.Application.SpecialDiet.Commands.CreateSpecialDiet;
using OrderEat.Application.SpecialDiet.Commands.DeleteSpecialDiet;
using OrderEat.Application.SpecialDiet.Commands.UpdateSpecialDiet;
using OrderEat.Application.SpecialDiet.Queries;
using OrderEat.Application.SpecialDiet.Queries.GetSpecialDietById;
using OrderEat.Application.SpecialDiet.Queries.GetSpecialDiets;
using OrderEat.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrderEat.Api.Controllers
{
    [ApiController]
    [Route("api/specialDiet")]
    [Authorize]
    public class SpecialDietController : Controller
    {
        private readonly IMediator _mediator;

        public SpecialDietController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPatch]
        [Route("{dietId}/dish/{dishId}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<ActionResult> AddDishToDiet([FromRoute] int dishId, [FromRoute] int dietId)
        {
            var request = new AddDietToDishCommand()
            {
                DietId = dietId,
                DishId = dishId
            };
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> Create([FromBody] CreateSpecialDietCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _mediator.Send(command);
            return Created($"api/specialDiet/{response}", null);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
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
        [AllowAnonymous]
        
        public async Task<ActionResult<PagedResult<GetDietDto>>> Get([FromQuery] GetSpecialDietsQuery dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var request = new GetSpecialDietsQuery()
            {
                SearchPhrase = dto.SearchPhrase,
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize,
                SortBy = dto.SortBy,
                SortDirection = dto.SortDirection
            };

            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<GetDietDto>> GetById([FromRoute] int id)
        {
            var request = new GetSpecialDietByIdQuery()
            {
                Id = id
            };

            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] DietDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var request = new UpdateSpecialDietCommand()
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
            };

            await _mediator.Send(request);

            return Ok();
        }
    }
}