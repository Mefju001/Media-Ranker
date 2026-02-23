using Application.Common.DTO.Request;
using Application.Features.ReviewServices.DeleteReviewAsync;
using Application.Features.ReviewServices.GetAllReviewsAsync;
using Application.Features.ReviewServices.GetByIdReview;
using Application.Features.ReviewServices.GetTheLastestReview;
using Application.Features.ReviewServices.UpsertReview;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator mediator;
        public ReviewController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        private Guid? getUserId()
        {
            var stringUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (stringUserId == null) return null;
            if (Guid.TryParse(stringUserId, out var userId))
            {
                return userId;
            }
            else
                return null;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllReviewsQuery();
            var reviews = await mediator.Send(query);
            return Ok(reviews);
        }
        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        public async Task<IActionResult> AddReview([FromQuery] int movieId, [FromQuery] ReviewRequest reviewRequest)
        {
            var userId = getUserId();
            if (userId == null) return Unauthorized();
            var command = new ReviewUpsertCommand
            (
                null,
                movieId,
                userId.Value,
                reviewRequest.Rating,
                reviewRequest.Comment
            );
            var response = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { response.id }, response);
        }
        [Authorize(Roles = "User, Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateReview([FromQuery] int reviewId, [FromQuery] int movieId, [FromQuery] ReviewRequest reviewRequest)
        {
            var userId = getUserId();
            if (userId == null) return Unauthorized();
            var command = new ReviewUpsertCommand
            (
                reviewId,
                movieId,
                userId.Value,
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
            var query = new GetTheLastestQuery();
            var reviews = await mediator.Send(query);
            return Ok(reviews);
        }
        [Authorize(Roles = "User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetByIdReviewQuery(id);
            return Ok(await mediator.Send(query));
        }
        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var userId = getUserId();
            if (userId == null) return Unauthorized();
            var command = new DeleteReviewCommand(id);
            await mediator.Send(command);
            return NoContent();
        }
    }
}
