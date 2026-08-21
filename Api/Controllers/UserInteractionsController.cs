using Application.Features.Common.Interfaces;
using Application.Features.UserInteractions.Statuses.Common;
using Application.Features.UserInteractions.Statuses.GetAll;
using Application.Features.UserInteractions.Statuses.SetStatus;
using Application.Features.UserInteractions.Statuses.DeleteById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Enums;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserInteractionsController:ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUserContext currentUserContext;
        public UserInteractionsController(IMediator mediator, ICurrentUserContext currentUserContext)
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
        public async Task<IActionResult> GetAll([FromQuery(Name = "ratingVote")] ERatingVote? eRatingVote, [FromQuery(Name = "typeInteractions")] ETypeInteractions? eTypeInteractions)
        {
            var userId = GetCurrentUserId();
            var query = new GetAllQuery(userId, eRatingVote, eTypeInteractions);
            var responses = await mediator.Send(query);
            return Ok(responses);
        }
        [ProducesResponseType(typeof(UserInteractionsRequest), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserInteractionsRequest liked)
        {
            var userId = GetCurrentUserId();
            var command = new SetStatusCommand(userId, liked.MediaId, liked.RatingVote, liked.TypeInteractions);
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
