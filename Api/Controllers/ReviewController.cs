using Application.Common.DTO.Request;
using Application.Common.Interfaces;
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
            var query = new GetAllReviewsQuery();
            var reviews = await mediator.Send(query);
            return Ok(reviews);
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AddReview([FromQuery] int movieId, [FromQuery] ReviewRequest reviewRequest)
        {
            var userId = GetCurrentUserId();
            var command = new ReviewUpsertCommand
            (
                null,
                movieId,
                userId,
                reviewRequest.Rating,
                reviewRequest.Comment
            );
            var response = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { response.id }, response);
        }
        [Authorize(Roles = "User")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateReview([FromRoute] int id, [FromQuery] ReviewRequest reviewRequest)
        {
            var userId = GetCurrentUserId();
            var command = new ReviewUpsertCommand
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
            var query = new GetTheLastestQuery();
            var reviews = await mediator.Send(query);
            return Ok(reviews);
        }
        [Authorize(Roles = "User")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetByIdReviewQuery(id);
            return Ok(await mediator.Send(query));
        }
        [Authorize(Roles = "User")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteReviewCommand(id);
            await mediator.Send(command);
            return NoContent();
        }
    }
}
