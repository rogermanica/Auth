using System.Threading.Tasks;
using AuthApi.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("accounts")]
        public async Task<IActionResult> ListUsers()
        {
            var command = new QueryAllUsers();
            var response = await _mediator.Send(command);

            return Ok(response.Value);
        }

        [HttpDelete, Route("accounts/{accountId}")]
        public async Task<IActionResult> DeleteAccount(int accountId)
        {
            var command = new RemoveAccount(accountId);
            var response = await _mediator.Send(command);

            if (response.HasMessages)
            {
                return BadRequest(response.Messages);
            }
            return Ok();
        }

        [HttpPost, AllowAnonymous, Route("register")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser command)
        {
            var response = await _mediator.Send(command);
            if (response.HasMessages)
            {
                return BadRequest(response.Messages);
            }

            return Created("accounts/profile", response.Value);
        }

        [HttpPost, AllowAnonymous, Route("login")]
        public async Task<IActionResult> Authenticate([FromBody] Authenticate command)
        {
            var response = await _mediator.Send(command);
            if (response.HasMessages)
            {
                return BadRequest(response.Messages);
            }

            return Ok(response.Value);
        }

        [HttpGet, Route("profile")]
        public async Task<IActionResult> Get()
        {
            var response = await _mediator.Send(new QueryUserProfile());

            if (response == null)
            {
                return NoContent();
            }

            return Ok(response.Value);
        }

        [HttpPut, Route("profile/password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangeUserPassword command)
        {
            var response = await _mediator.Send(command);

            if (response.HasMessages)
            {
                return BadRequest(response.Messages);
            }

            return Ok(response.Value);
        }
    }
}