using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderEat.Application.Order.Command.AddDishToOrder;
using OrderEat.Application.Order.Command.CreateOrder;
using OrderEat.Application.Order.Command.RealizeOrder;
using OrderEat.Application.Order.Query.GetById;
using OrderEat.Application.Order.Query.GetOrdersToRealizeFromRestaurant;
using OrderEat.Domain.Entities;
using OrderEat.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

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

        [HttpPatch]
        [Route("order/{orderId}/dish/{dishId}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Add customized dish to order")]
        public async Task<ActionResult> AddDishToOrder([FromRoute] int orderId, [FromRoute] int dishId, [FromBody] Sheet additionalIngredient)
        {
            var command = new AddDishToOrderCommand()
            {
                OrderId = orderId,
                DishId = dishId,
                AditionalIngrediens = additionalIngredient
            };

            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route("restaurant/{id}/order")]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [SwaggerOperation("Create order to restaurant")]
        public async Task<ActionResult<string>> Create([FromRoute] int id)
        {
            var command = new CreateOrderCommand()
            {
                RestaurantId = id
            };
            var resourceId = await _mediator.Send(command);
            return Created($"/api/order/{resourceId}", null);
        }

        [HttpGet]
        [Route("order/{id}")]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Get order by id")]
        public async Task<ActionResult<Order>> GetById([FromRoute] int id)
        {
            var query = new GetByIdQuery()
            {
                Id = id
            };

            var order = await _mediator.Send(query);
            return Ok(order);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        [Route("restaurant/{restaurantId}/orderedOrder")]
        [ProducesResponseType(typeof(PagedResult<Order>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Get orders from restaurant which status is ordered")]
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

        [HttpPatch]
        [Route("order/{orderId}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation($"Change order status to ordered")]
        public async Task<ActionResult> RealizeOrder([FromRoute] int orderId)
        {
            var command = new RealizeOrderCommand()
            {
                Orderid = orderId
            };

            await _mediator.Send(command);
            return Ok();
        }
    }
}