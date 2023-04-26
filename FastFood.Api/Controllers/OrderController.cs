using FastFood.Application.Order.Command.CreateOrder;
using FastFood.Application.Order.Query.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
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
            var command = new CreateOrderCommand()
            {
                RestaurantId = id
            };
            var resourceId = await _mediator.Send(command);
            return $"/api/order/{resourceId}";
        }

        [HttpGet]
        [Route("order/{id}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var query = new GetByIdQuery()
            {
                Id = id
            };

            var order = await _mediator.Send(query);
            return Ok(order);
        }
    }
}