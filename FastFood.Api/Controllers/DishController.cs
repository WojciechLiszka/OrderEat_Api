using FastFood.Application.Dish;
using FastFood.Application.Dish.Command.CreateDish;
using FastFood.Application.Dish.Command.UpdateDish;
using FastFood.Application.Dish.Queries;
using FastFood.Application.Dish.Queries.GedDishesFromRestaurant;
using FastFood.Application.Dish.Queries.GetDishById;
using FastFood.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("/api")]
    public class DishController : Controller
    {
        private readonly IMediator _mediator;

        public DishController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("/restaurant/{restaurantid}/dish")]
        public async Task<ActionResult<string>> Create([FromRoute] int restaurantid, [FromQuery] DishDto dto)
        {
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
            return Created($"/api/restaurant/{restaurantid}/dish", null);
        }

        [HttpGet]
        [Route("dish/{id}")]
        public async Task<ActionResult<GetDishDto>> GetById([FromRoute] int id)
        {
            var request = new GetDishByIdQuery()
            {
                DishId = id
            };

            var dto = await _mediator.Send(request);

            return Ok(dto);
        }

        [HttpPut]
        [Route("dish/{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromQuery] DishDto dto)
        {
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

        [HttpGet]
        [Route("/restaurant/{restaurantid}/dish")]
        public async Task<ActionResult<PagedResult<GetDishDto>>> GetFromRestaurant([FromRoute] int restaurantid, [FromQuery] PagedResultDto dto)
        {
            var request = new GetDishesFromRestaurantQuery()
            {
                RestaurantId = restaurantid,
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