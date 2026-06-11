using Application.Features.Common.Interfaces;
using Application.Features.Liked.Common;
using Application.Features.Watched.Add;
using Application.Features.Watched.DeleteById;
using Application.Features.Watched.GetAll;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WatchedController:ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUserContext currentUserContext;
        public WatchedController(IMediator mediator, ICurrentUserContext currentUserContext)
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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetCurrentUserId();
            var query = new GetAllQuery(userId);
            var movies = await mediator.Send(query);
            return Ok(movies);
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
