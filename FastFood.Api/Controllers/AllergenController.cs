using FastFood.Application.Allergen.Commands.CreateAllergen;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("api/allergen")]
    public class AllergenController : Controller
    {
        private readonly IMediator _mediator;

        public AllergenController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateAllergenCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}