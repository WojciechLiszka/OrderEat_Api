using OrderEat.Application.Account.Command;
using OrderEat.Application.Account.Command.DeleteUser;
using OrderEat.Application.Account.Command.RegisterUser;
using OrderEat.Application.Account.Command.UpdateUserDetails;
using OrderEat.Application.Account.Commands.UpdateUserRole;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OrderEat.Api.Controllers
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

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] RegisterUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _mediator.Send(command);
            return Ok();
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
    }
}