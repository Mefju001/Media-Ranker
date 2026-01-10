using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Services.Interfaces;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/Liked")]
    public class LikedMovieGameTvSeriesController : ControllerBase
    {
        private readonly ILikedMediaServices likedMediaServices;
        private int? getUserId()
        {
            var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrWhiteSpace(stringUserId)) return null;
            if (int.TryParse(stringUserId, out int userId))
            {
                return userId;
            }
            else
                return null;
        }
        public LikedMovieGameTvSeriesController(ILikedMediaServices likedMediaServices)
        {
            this.likedMediaServices = likedMediaServices;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var movies = await likedMediaServices.GetAllAsync();
            return Ok(movies);
        }
        [Authorize(Roles = "User")]
        [HttpGet("getLikedByUser")]
        public async Task<IActionResult> GetLikedByUser()
        {
            var userId = getUserId();
            if (userId is null) return Unauthorized();
            return Ok(await likedMediaServices.GetUserLikedMedia(userId.Value));
        }
        [Authorize(Roles = "User")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(LikedMediaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await likedMediaServices.GetBy(id);
            if (response is null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(typeof(LikedMediaRequest), StatusCodes.Status201Created)]
        [HttpPost("Add")]
        public async Task<IActionResult> AddLikedMovie([FromBody] LikedMediaRequest likedMovie)
        {
            var userId = getUserId();
            if (userId is null) return Unauthorized();
            likedMovie.UserId = userId.Value;
            var response = await likedMediaServices.Add(likedMovie);
            return CreatedAtAction(nameof(GetById), new { id = response.id }, response);
        }
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteLikedMovie([FromRoute] int id)
        {
            var userId = getUserId();
            if (userId is null) return Unauthorized();
            await likedMediaServices.Delete(userId.Value, id);
            return NoContent();
        }
    }
}
