using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.DTO.Request;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/Liked")]
    public class LikedMovieGameTvSeries : ControllerBase
    {
        private readonly ILikedMediaServices likedMediaServices;
        private int getUserId()
        {
            var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return parse(stringUserId);
        }
        private int parse(string? String)
        {
            if (String.IsNullOrWhiteSpace(String)) throw new ArgumentNullException(nameof(String));
            if (int.TryParse(String, out int userId))
            {
                return userId;
            }
            else
                throw new ArgumentException("Nieprawidłowy format identyfikatora. Wymagana liczba całkowita.", nameof(String));
        }
        public LikedMovieGameTvSeries(ILikedMediaServices likedMediaServices)
        {
            this.likedMediaServices = likedMediaServices;
        }
        [HttpGet("/All")]
        public async Task<IActionResult> GetAll()
        {
            var movies = await likedMediaServices.GetAllAsync();
            return Ok(movies);
        }
        [Authorize(Roles = "User")]
        [HttpGet("/getLikedByUser")]
        public async Task<IActionResult> GetLikedByUser()
        {
            var userId = getUserId();
            return Ok(await likedMediaServices.GetUserLikedMedia(userId));
        }
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(typeof(LikedMediaRequest),StatusCodes.Status200OK)]
        [HttpPost("/Add")]
        public async Task<IActionResult> AddLikedMovie([FromBody] LikedMediaRequest likedMovie)
        {
            var response = await likedMediaServices.Add(likedMovie);
            return Ok(response);
            //return CreatedAtAction(nameof(GetById), new { id = response.Media. }, response);
        }
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("/delete/{id}")]
        public async Task<IActionResult> DeleteLikedMovie(int id)
        {
            var userId = getUserId();
            await likedMediaServices.Delete(userId, id);
            return NoContent();
        }
    }
}
