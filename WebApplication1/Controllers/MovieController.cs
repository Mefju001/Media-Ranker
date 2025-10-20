using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO.Request;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieServices movieServices;

        public MovieController(IMovieServices Movservices)
        {
            movieServices = Movservices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await movieServices.GetAllAsync();
            return Ok(movies);
        }
        [HttpGet("sortBy/{sortByField}/{sortByDirection}")]
        public async Task<IActionResult> GetSortAll(string sortByField, string sortByDirection)
        {
            var movies = await movieServices.GetSortAll(sortByField,sortByDirection);
            return Ok(movies);
        }
        [HttpGet("FilterBy")]
        public async Task<IActionResult> GetMovies([FromQuery] string? name, [FromQuery] string? genreName, [FromQuery] string? directorName, [FromQuery] int? movieId)
        {
            var movies = await movieServices.GetMovies(name, genreName, directorName, movieId);
            return Ok(movies);
        }
        [HttpGet("byAvarage")]
        public async Task<IActionResult> GetMoviesByAvarage()
        {
            var movies = await movieServices.GetMoviesByAvrRating();
            return Ok(movies);
        }
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await movieServices.GetById(id);
            return Ok(movie);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Upsert(int? id, MovieRequest movie)
        {
            var created = await movieServices.Upsert(id, movie);
            if (id is null)
                return CreatedAtAction(nameof(GetById), new { id = created.movieId }, created.response);
            return Ok(created.response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await movieServices.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

    }
}