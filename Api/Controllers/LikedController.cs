using Application.Features.Common.Interfaces;
using Application.Features.Liked.Add;
using Application.Features.Liked.Common;
using Application.Features.Liked.DeleteById;
using Application.Features.Liked.GetAll;
using Application.Features.Liked.GetAllForUser;
using Application.Features.Liked.GetById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LikedController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUserContext currentUserContext;
        public LikedController(IMediator mediator, ICurrentUserContext currentUserContext)
        {
            this.mediator = mediator;
            this.currentUserContext = currentUserContext;
        }
        private Guid GetCurrentUserId()
        {
            var userId = currentUserContext.UserId;
            if (userId is null) throw new UnauthorizedAccessException();
            return userId.Value;
        }
        [HttpGet("ForUser")]
        public async Task<IActionResult> GetLikedByUser()
        {
            var userId = GetCurrentUserId();
            var query = new GetAllForUserQuery(userId);
            return Ok(await mediator.Send(query));
        }
        [ProducesResponseType(typeof(UserInteractionsRequest), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserInteractionsRequest liked)
        {
            var userId = GetCurrentUserId();
            var command = new AddCommand(userId, liked.MediaId);
            var response = await mediator.Send(command);
            return Ok();
        }
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            var userId = GetCurrentUserId();
            var command = new DeleteByIdCommand(userId, id);
            await mediator.Send(command);
            return NoContent();
        }
    }
}
