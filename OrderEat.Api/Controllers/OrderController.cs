using OrderEat.Application.Order.Command.AddDishToOrder;
using OrderEat.Application.Order.Command.CreateOrder;
using OrderEat.Application.Order.Command.RealizeOrder;
using OrderEat.Application.Order.Query.GetById;
using OrderEat.Application.Order.Query.GetOrdersToRealizeFromRestaurant;
using OrderEat.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OrderEat.Api.Controllers
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

        [HttpPatch]
        [Route("order/{orderId}/dish/{dishId}")]
        public async Task<ActionResult> AddDishToOrder([FromRoute] int orderId, [FromRoute] int dishId, [FromBody] Sheet aditionalIngredient)
        {
            var command = new AddDishToOrderCommand()
            {
                OrderId = orderId,
                DishId = dishId,
                AditionalIngrediens = aditionalIngredient
            };

            await _mediator.Send(command);
            return Ok();
        }

        [HttpPatch]
        [Route("order/{orderId}")]
        public async Task<ActionResult> RealizeOrder([FromRoute] int orderId)
        {
            var command = new RealizeOrderCommand()
            {
                Orderid = orderId
            };

            await _mediator.Send(command);
            return Ok();
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        [Route("restaurant/{restaurantId}/orderedOrder")]
        public async Task<ActionResult> GetOrdersToRealize([FromBody] int restaurantId, [FromQuery] PagedResultDto queryParams)
        {
            var command = new GetOrdersToRealizeFromRestaurantQuery()
            {
                RestaurantId = restaurantId,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                SearchPhrase = queryParams.SearchPhrase,
                SortBy = queryParams.SortBy,
                SortDirection = queryParams.SortDirection,
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}