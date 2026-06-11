using Application.Features.Common.Interfaces;
using Application.Features.Disliked.Add;
using Application.Features.Disliked.DeleteById;
using Application.Features.Liked.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DislikedController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUserContext currentUserContext;
        public DislikedController(IMediator mediator, ICurrentUserContext currentUserContext)
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
