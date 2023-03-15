using FastFood.Application.Account.Command;
using FastFood.Application.Account.Command.RegisterUser;
using FastFood.Application.Account.Command.UpdateUserDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login([FromQuery] LoginUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
       // [Authorize]
        public async Task<ActionResult> Update([FromQuery] UpdateUserDetailsCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}