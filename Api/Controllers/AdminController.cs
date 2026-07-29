using Application.Features.AdminPanel.ChangeDetails;
using Application.Features.AdminPanel.ChangePassword;
using Application.Features.AdminPanel.DeleteById;
using Application.Features.AdminPanel.GetAll;
using Application.Features.User.GetUserExcludeAdmin;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IMediator mediator;
        public AdminController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetNumbers()
        {
            var results = await mediator.Send(new GetAllQuery());
            return Ok(results);
        }
        [Authorize]
        [HttpGet("users")]
        public async Task<IActionResult> getUsers()
        {
            var results = await mediator.Send(new GetUserExcludeAdminQuery());
            return Ok(results);
        }
        [Authorize]
        [HttpDelete("users/{id:guid}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            var result = await mediator.Send(new DeleteByIdCommand(id));
            return Ok(result);
        }
        [Authorize]
        [HttpPut("users/{id}")]
        public async Task<IActionResult> ChangeDetails([FromRoute] Guid id, [FromBody] ChangeDetailsRequest request)
        {
            await mediator.Send(new ChangeDetailsCommand(id,request.name,request.surname));
            return Ok();
        }
        [Authorize]
        [HttpPut("users/{id}/password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            await mediator.Send(command);
            return Ok();
        }
    }
}
