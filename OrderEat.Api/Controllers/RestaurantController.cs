using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderEat.Application.Restaurant;
using OrderEat.Application.Restaurant.Commands.CreateRestaurant;
using OrderEat.Application.Restaurant.Commands.DeleteRestaurant;
using OrderEat.Application.Restaurant.Commands.UpdateRestaurant;
using OrderEat.Application.Restaurant.Queries;
using OrderEat.Application.Restaurant.Queries.GetRestaurantById;
using OrderEat.Application.Restaurant.Queries.GetRestaurants;
using OrderEat.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderEat.Api.Controllers
{
    [ApiController]
    [Route("/api/restaurant")]
    [Authorize]
    public class RestaurantController : Controller
    {
        private readonly IMediator _mediator;

        public RestaurantController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 201)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Create restaurant")]
        public async Task<ActionResult<string>> Create([FromBody] CreateRestaurantCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var id = await _mediator.Send(command);
            return Created($"api/restaurant/{id}", null);
        }

        [HttpDelete]
        [Route("/api/restaurant/{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType(typeof(string), 204)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Delete restaurant")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var request = new DeleteRestaurantCommand()
            {
                Id = id
            };

            await _mediator.Send(request);

            return NoContent();
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedResult<GetRestaurantDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [SwaggerOperation("Get restaurant by query")]
        public async Task<ActionResult<PagedResult<GetRestaurantDto>>> Get([FromQuery] GetRestaurantsQuery query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        [Route("/api/restaurant/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(GetRestaurantDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Get restaurant by id")]
        public async Task<ActionResult<GetRestaurantDto>> GetById([FromRoute] int id)
        {
            var request = new GetRestaurantByIdQuery()
            {
                Id = id
            };
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPut]
        [Route("/api/restaurant/{id}")]
        [Authorize(Roles = "Admin,Owner")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Update restaurant details")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateRestaurantDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var request = new UpdateRestaurantCommand()
            {
                Id = id,
                Description = dto.Description,
                ContactNumber = dto.ContactNumber,
                Email = dto.Email,
                Country = dto.Country,
                City = dto.City,
                Street = dto.Street,
                ApartmentNumber = dto.ApartmentNumber
            };

            await _mediator.Send(request);

            return Ok();
        }
    }
}