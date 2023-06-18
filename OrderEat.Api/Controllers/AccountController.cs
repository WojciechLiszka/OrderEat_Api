using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderEat.Application.Account.Command;
using OrderEat.Application.Account.Command.DeleteUser;
using OrderEat.Application.Account.Command.RegisterUser;
using OrderEat.Application.Account.Command.UpdateUserDetails;
using OrderEat.Application.Account.Commands.UpdateUserRole;
using Swashbuckle.AspNetCore.Annotations;

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
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [Route("{userEmail}/role/{roleId}")]
        [Authorize("Admin")]
        [SwaggerOperation("Changes user role")]
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
        [ProducesResponseType(typeof(string), 204)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [SwaggerOperation("Delete user account")]
        [Authorize]
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
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [SwaggerOperation("Login to user account")]
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
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [SwaggerOperation("Register user account")]
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
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [SwaggerOperation("Change user details")]
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