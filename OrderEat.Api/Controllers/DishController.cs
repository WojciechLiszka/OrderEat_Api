using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderEat.Application.Dish.Command.CreateDish;
using OrderEat.Application.Dish.Command.DeleteDish;
using OrderEat.Application.Dish.Command.UpdateDish;
using OrderEat.Application.Dish.Queries.GedDishesFromRestaurant;
using OrderEat.Application.Dish.Queries.GetDishById;
using OrderEat.Application.Dish.Queries.SmartSearchDish;
using OrderEat.Application.Dish.Queries.SmartSearchDishFromRestaurant;
using OrderEat.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderEat.Api.Controllers
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
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Crate dish in restaurant")]
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
        [ProducesResponseType(typeof(string), 204)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Delete dish")]
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
        [ProducesResponseType(typeof(Application.Dish.GetDishDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Get dish by id")]
        public async Task<ActionResult<Application.Dish.GetDishDto>> GetById([FromRoute] int Id)
        {
            var request = new GetDishByIdQuery()
            {
                DishId = Id
            };
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpGet]
        [Route("restaurant/{restaurantId}/dish")]
        [ProducesResponseType(typeof(PagedResult<Application.Dish.GetDishDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Get dishes from restaurant")]
        public async Task<ActionResult<PagedResult<Application.Dish.GetDishDto>>> GetFromRestaurant([FromRoute] int restaurantId, [FromQuery] PagedResultDto dto)
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
        [Authorize]
        [ProducesResponseType(typeof(PagedResult<Application.Dish.GetDishDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Get user personalized dishes from restaurant")]
        public async Task<ActionResult<PagedResult<Application.Dish.GetDishDto>>> GetSmartFromRestaurant([FromRoute] int restaurantId, [FromQuery] PagedResultDto dto)
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
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Update dish details")]
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