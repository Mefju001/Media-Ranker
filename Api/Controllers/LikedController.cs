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
    [Authorize(Roles = "User")]
    [ApiController()]
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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllQuery();
            var movies = await mediator.Send(query);
            return Ok(movies);
        }
        [HttpGet("ForUser")]
        public async Task<IActionResult> GetLikedByUser()
        {
            var userId = GetCurrentUserId();
            var query = new GetAllForUserQuery(userId);
            return Ok(await mediator.Send(query));
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(LikedResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var query = new GetByIdQuery(id);
            var response = await mediator.Send(query);
            if (response is null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [ProducesResponseType(typeof(LikedMediaRequest), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] LikedMediaRequest liked)
        {
            var userId = GetCurrentUserId();
            var command = new AddCommand(userId, liked.MediaId);
            var response = await mediator.Send(command);
            return Ok();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteById([FromRoute] Guid id)
        {
            var userId = GetCurrentUserId();
            var command = new DeleteByIdCommand(userId, id);
            await mediator.Send(command);
            return NoContent();
        }
    }
}
