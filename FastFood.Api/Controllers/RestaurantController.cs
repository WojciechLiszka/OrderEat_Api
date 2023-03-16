using FastFood.Application.Restaurant.Commands.CreateRestaurant;
using FastFood.Application.Restaurant.Queries;
using FastFood.Application.Restaurant.Queries.GetRestaurantById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("/api/restaurant")]
    public class RestaurantController : Controller
    {
        private readonly IMediator _mediator;

        public RestaurantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([FromQuery] CreateRestaurantCommand command)
        {
            var id = await _mediator.Send(command);
            return Created($"api/restaurant/{id}", null);
        }

        [HttpGet]
        [Route("/api/restaurant/{id}")]
        public async Task<ActionResult<GetRestaurantDto>> GetById([FromRoute] int id)
        {
            var request = new GetRestaurantByIdQuery()
            {
                Id = id
            };
            var result = await _mediator.Send(request);
            return Ok(result);
        }
    }
}