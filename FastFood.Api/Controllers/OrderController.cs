using FastFood.Application.Order.CreateOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class OrderController : Controller
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("restaurant/{id}/order")]
        public async Task<ActionResult<string>> Create([FromRoute] int id)
        {
            var Command = new CreateOrderCommand()
            {
                RestaurantId = id
            };
            var resourceId =await _mediator.Send(Command);
            return $"place/holder/{resourceId}";
        }
    }
}