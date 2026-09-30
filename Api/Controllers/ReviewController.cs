using Application.Features.Common.Interfaces;
using Application.Features.UserInteractions.Reviews.DeleteById;
using Application.Features.UserInteractions.Reviews.GetAll;
using Application.Features.UserInteractions.Reviews.GetById;
using Application.Features.UserInteractions.Reviews.GetTheLastestTitle;
using Application.Features.UserInteractions.Reviews.Upsert;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUserContext currentUserContext;
        public ReviewController(IMediator mediator, ICurrentUserContext currentUserContext)
        {
            this.mediator = mediator;
            this.currentUserContext = currentUserContext;
        }
        private Guid GetCurrentUserId()
        {
            var userId = currentUserContext.UserId;
            if (userId == null) throw new UnauthorizedAccessException();
            return userId.Value;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllQuery();
            var reviews = await mediator.Send(query);
            return Ok(reviews);
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] ReviewRequest reviewRequest)
        {
            var userId = GetCurrentUserId();
            var command = new UpsertCommand
            (
                null,
                reviewRequest.MovieId,
                userId,
                reviewRequest.Rating,
                reviewRequest.Comment
            );
            var response = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { response.id }, response);
        }
        [Authorize(Roles = "User")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateReview([FromRoute] Guid id, [FromBody] ReviewRequest reviewRequest)
        {
            var userId = GetCurrentUserId();
            var command = new UpsertCommand
            (
                id,
                null,
                null,
                reviewRequest.Rating,
                reviewRequest.Comment
            );
            var response = await mediator.Send(command);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("TheLatest")]
        public async Task<IActionResult> GetAllSortedByLatestAsync()
        {
            var query = new GetTheLastestTitleQuery();
            var reviews = await mediator.Send(query);
            return Ok(reviews);
        }
        [Authorize(Roles = "User")]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var query = new GetByIdQuery(id);
            return Ok(await mediator.Send(query));
        }
        [Authorize(Roles = "User")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid mediaId, [FromRoute] Guid id)
        {
            var command = new DeleteByIdCommand(mediaId, id);
            await mediator.Send(command);
            return NoContent();
        }
    }
}
