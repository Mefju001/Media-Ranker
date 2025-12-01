using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.DTO.Request;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewServices services;
        public ReviewController(IReviewServices services)
        {
            this.services = services;
        }
        private int? getUserId()
        {
            var stringUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(stringUserId == null)return null;
            int id = int.Parse(stringUserId);
            if (int.TryParse(stringUserId, out int userId))
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
            var reviews = await services.GetAllAsync();
            return Ok(reviews);
        }
        [Authorize(Roles = "User, Admin")]
        [HttpPost]
        public async Task<IActionResult> AddReview([FromQuery] int movieId, [FromQuery] ReviewRequest reviewRequest)
        {
            var userId = getUserId();
            if (userId == null) return Unauthorized();
            var response = await services.Upsert(null, userId.Value, movieId, reviewRequest);
            return CreatedAtAction(nameof(GetById), new { response.id }, response);
        }
        [Authorize(Roles = "User, Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateReview([FromQuery]int reviewId, [FromQuery] int movieId, [FromQuery] ReviewRequest reviewRequest)
        {
            var userId = getUserId();
            if (userId == null) return Unauthorized();
            var response = await services.Upsert(reviewId, userId.Value, movieId, reviewRequest);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpGet("TheLatest")]
        public async Task<IActionResult> GetAllSortedByLatestAsync()
        {
            var reviews = await services.GetTheLatestReviews();
            return Ok(reviews);
        }
        [Authorize(Roles = "User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await services.GetById(id));
        }
        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var userId = getUserId();
            if (userId == null) return Unauthorized();
            var deletedReview = await services.Delete(id, userId.Value);
            if (!deletedReview) return NotFound();
            return NoContent();
        }

    }
}
