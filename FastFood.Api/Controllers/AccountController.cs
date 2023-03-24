using FastFood.Application.Account.Command;
using FastFood.Application.Account.Command.DeleteUser;
using FastFood.Application.Account.Command.RegisterUser;
using FastFood.Application.Account.Command.UpdateUserDetails;
using FastFood.Application.Account.Commands.UpdateUserRole;
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
        public async Task<ActionResult> Register([FromBody] RegisterUserCommand command)
        {
            
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UpdateUserDetailsCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _mediator.Send(command);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromBody] DeleteUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPatch]
        [Route("{userEmail}/role/{roleId}")]
        public async Task<ActionResult> ChangeRole([FromRoute] string userEmail, [FromRoute] int roleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var request = new UpdateUserRoleCommand()
            {
                RoleId = roleId,
                UserEmail = userEmail
            };
            await _mediator.Send(request);
            return Ok();
        }
    }
}