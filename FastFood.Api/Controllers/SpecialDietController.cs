using FastFood.Application.Dish.Command.AddDietToDish;
using FastFood.Application.SpecialDiet.Commands;
using FastFood.Application.SpecialDiet.Commands.CreateSpecialDiet;
using FastFood.Application.SpecialDiet.Commands.DeleteSpecialDiet;
using FastFood.Application.SpecialDiet.Commands.UpdateSpecialDiet;
using FastFood.Application.SpecialDiet.Queries;
using FastFood.Application.SpecialDiet.Queries.GetSpecialDietById;
using FastFood.Application.SpecialDiet.Queries.GetSpecialDiets;
using FastFood.Domain.Models;
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

        [HttpPatch]
        [Route("{dietId}/dish/{dishId}")]
        public async Task<ActionResult> AddDietToDish([FromRoute] int dishId, [FromRoute] int dietId)
        {
            var request = new AddDietToDishCommand()
            {
                DietId = dietId,
                DishId = dishId
            };
            await _mediator.Send(request);
            return Ok();
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

        [HttpGet]
        public async Task<ActionResult<PagedResult<GetDietDto>>> Get([FromQuery] PagedResultDto dto)
        {
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
    }
}