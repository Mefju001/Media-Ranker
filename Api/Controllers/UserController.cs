using Application.Common.DTO.Request;
using Application.Features.UserServices.ChangeDetails;
using Application.Features.UserServices.ChangePassword;
using Application.Features.UserServices.GetBy;
using Application.Features.UserServices.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    /*            httpContextAccessor.HttpContext!.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.Now.AddDays(1)
            });*/
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        private Guid? getUserId()
        {
            var stringUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(stringUserId, out var userId))
                throw new ArgumentException();
            return userId;
        }
        /*[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var query = new GetAll
            return Ok(await userServices.GetAllAsync());
        }*/
    [Authorize(Roles = "Admin")]
        [HttpGet("id:{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var query = new GetByIdQuery(id);
            return Ok(mediator.Send(query));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetBy(string name)
        {
            var query = new GetUserByNameQuery(name);
            return Ok(mediator.Send(query));
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPatch("ChangePassword")]
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
        [HttpPatch("ChangeDetails")]
        public async Task<IActionResult> ChangeDetails([FromBody] UserDetailsRequest userDetailsRequest)
        {
            var userId = getUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            var command = new ChangeDetailsCommand(userId.Value, userDetailsRequest.name,userDetailsRequest.surname,userDetailsRequest.email);
            await mediator.Send(command);
            return Ok();
        }
    }
}
