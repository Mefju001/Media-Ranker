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
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await services.GetAllAsync();
            return Ok(reviews);

        }
        private int parse(string String)
        {
            int id = int.Parse(String);
            if (int.TryParse(String, out int userId))
            {
                return userId;
            }
            else
                throw new Exception();
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Upsert(int? reviewId, int movieId, ReviewRequest reviewRequest)
        {
            var stringUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (stringUserId == null)
            {
                return Unauthorized();
            }
            int userId = parse(stringUserId);
            var (id, response) = await services.Upsert(reviewId, userId, movieId, reviewRequest);
            if (reviewId is null)
            {
                return CreatedAtAction(nameof(GetById), new { id }, response);
            }

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
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedReview = await services.Delete(id);
            if (!deletedReview) return NotFound();
            return NoContent();
        }

    }
}
