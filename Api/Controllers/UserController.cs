using Application.Common.DTO.Request;
using Application.Common.Interfaces;
using Application.Features.UserServices.ChangeDetails;
using Application.Features.UserServices.ChangePassword;
using Application.Features.UserServices.GetBy;
using Application.Features.UserServices.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "Admin,User")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUserContext currentUserContext;
        public UserController(IMediator mediator, ICurrentUserContext currentUserContext)
        {
            this.mediator = mediator;
            this.currentUserContext = currentUserContext;
        }
        private Guid? getUserId()
        {
            var userId = currentUserContext.UserId;
            if (userId == null)
                throw new UnauthorizedAccessException();
            return userId.Value;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var query = new GetByIdQuery(id);
            return Ok(mediator.Send(query));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{name}")]
        public async Task<IActionResult> GetBy([FromRoute] string name)
        {
            var query = new GetUserByNameQuery(name);
            var result = await mediator.Send(query);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPatch("Change/Password")]
        public async Task<IActionResult> ChangePassword(string newPassword, string confirmPassword, string oldPassword)
        {
            var userId = getUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new ChangePasswordCommand(newPassword, confirmPassword, oldPassword, userId.Value);
            await mediator.Send(command);
            return Ok();
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPatch("Change/Details")]
        public async Task<IActionResult> ChangeDetails([FromBody] UserDetailsRequest userDetailsRequest)
        {
            var userId = getUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new ChangeDetailsCommand(userId.Value, userDetailsRequest.name, userDetailsRequest.surname, userDetailsRequest.email);
            await mediator.Send(command);
            return Ok();
        }
    }
}
