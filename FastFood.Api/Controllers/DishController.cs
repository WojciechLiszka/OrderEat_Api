using FastFood.Application.Dish;
using FastFood.Application.Dish.Command.CreateDish;
using FastFood.Application.Dish.Command.DeleteDish;
using FastFood.Application.Dish.Command.UpdateDish;
using FastFood.Application.Dish.Queries;
using FastFood.Application.Dish.Queries.GedDishesFromRestaurant;
using FastFood.Application.Dish.Queries.GetDishById;
using FastFood.Application.Dish.Queries.SmartSearchDish;
using FastFood.Application.Dish.Queries.SmartSearchDishFromRestaurant;
using FastFood.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class DishController : Controller
    {
        private readonly IMediator _mediator;

        public DishController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("restaurant/{restaurantid}/dish")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<ActionResult<string>> Create([FromRoute] int restaurantid, [FromBody] Application.Dish.GetDishDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var request = new CreateDishCommand()
            {
                RestaurantId = restaurantid,
                Name = dto.Name,
                Description = dto.Description,

                BasePrize = dto.BasePrize,
                BaseCaloricValue = dto.BaseCaloricValue,

                AllowedCustomization = dto.AllowedCustomization,
                IsAvilable = dto.IsAvilable,
            };

            var id = await _mediator.Send(request);

            return Created($"/api/dish/{id}", null);
        }

        [HttpDelete]
        [Route("dish/{id}")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var request = new DeleteDishCommand()
            {
                id = id
            };

            await _mediator.Send(request);

            return NoContent();
        }

        [HttpGet]
        [Route("dish/{id}")]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<Application.Dish.Queries.GetDishDto>> GetById([FromRoute] int id)
        {
            var request = new GetDishByIdQuery()
            {
                DishId = id
            };

            var dto = await _mediator.Send(request);

            return Ok(dto);
        }

        [HttpGet]
        [Route("restaurant/{restaurantId}/dish")]
        [ApiExplorerSettings(IgnoreApi = true)]

        public async Task<ActionResult<PagedResult<Application.Dish.Queries.GetDishDto>>> GetFromRestaurant([FromRoute] int restaurantId, [FromQuery] PagedResultDto dto)
        {
            var request = new GetDishesFromRestaurantQuery()
            {
                RestaurantId = restaurantId,
                SearchPhrase = dto.SearchPhrase,
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize,
                SortBy = dto.SortBy,
                SortDirection = dto.SortDirection
            };

            var validator = new GetDishesFromRestaurantQueryValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(request);

            return Ok(result);
        }
        [HttpGet]
        [Route("restaurant/{restaurantId}/dishSmart")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<PagedResult<Application.Dish.Queries.GetDishDto>>> GetSmartFromRestaurant([FromRoute] int restaurantId, [FromQuery] PagedResultDto dto)
        {
            var request = new SmartSearchDishFromRestaurantQuery()
            {
                RestaurantId = restaurantId,
                SearchPhrase = dto.SearchPhrase,
                PageNumber = dto.PageNumber,
                PageSize = dto.PageSize,
                SortBy = dto.SortBy,
                SortDirection = dto.SortDirection
            };

            var validator = new SmartSearchDishFromRestaurantQueryValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [HttpPut]
        [Route("dish/{id}")]
        [Authorize(Roles = "Admin,Owner")]
        
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] Application.Dish.GetDishDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var request = new UpdateDishCommand()
            {
                DishId = id,
                Name = dto.Name,
                Description = dto.Description,

                BasePrize = dto.BasePrize,
                BaseCaloricValue = dto.BaseCaloricValue,

                AllowedCustomization = dto.AllowedCustomization,
                IsAvilable = dto.IsAvilable
            };

            await _mediator.Send(request);

            return Ok();
        }
    }
}