using FastFood.Application.Restaurant.Commands.CreateRestaurant;
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
    }
}