using FastFood.Application.Order.Command.AddDishToOrder;
using FastFood.Application.Order.Command.CreateOrder;
using FastFood.Application.Order.Command.RealizeOrder;
using FastFood.Application.Order.Query.GetById;
using FastFood.Application.Order.Query.GetOrdersToRealizeFromRestaurant;
using FastFood.Domain.Models;
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
            var command = new OrderOrderCommand()
            {
                Orderid = orderId
            };

            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Owner")]
        [Route("restaurant/{restaurantId}/order")]
        public async Task<ActionResult<PagedResult<Domain.Entities.Order>>> GetSelctedOrdersFromRestaurant([FromBody] int restaurantId, [FromQuery] PagedResultDto queryParams, [FromQuery] OrderStatus selectedStatus)
        {
            var command = new GetSelectedOrdersRestaurantQuery()

            {
                RestaurantId = restaurantId,
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                SearchPhrase = queryParams.SearchPhrase,
                SortBy = queryParams.SortBy,
                SortDirection = queryParams.SortDirection,
                SelectedStatus = selectedStatus
            };

            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}