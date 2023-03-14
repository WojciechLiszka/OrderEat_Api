using FastFood.Application.Account.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Api.Controllers
{
    [ApiController]
    [Route("/api/account")]
    public class AccountController : Controller
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromQuery] RegisterUserCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}