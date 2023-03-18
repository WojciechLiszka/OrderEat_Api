using FastFood.Application.SpecialDiet.Commands.CreateSpecialDiet;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("api/specialDiet")]
    public class SpecialDietController : Controller
    {
        private readonly IMediator _mediator;

        public SpecialDietController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Create([FromQuery] CreateSpecialDietCommand command)
        {
            var response = await _mediator.Send(command);
            return Created($"api/specialDiet/{response}", null);
        }
    }
}