using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO.Request;
using WebApplication1.Models;
using WebApplication1.QueryHandler.Query;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieServices movieServices;

        public MovieController(IMovieServices Movservices)
        {
            movieServices = Movservices;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await movieServices.GetAllAsync();
            return Ok(movies);
        }
        [AllowAnonymous]
        [HttpGet("test")]
        public async Task<IActionResult> test()
        {
            var test = movieServices.testForReviews();
            return Ok(await test);
        }
        [AllowAnonymous]
        [HttpGet("FilterBy")]
        public async Task<IActionResult> GetMovies([FromQuery] MovieQuery moviesQuery)
        {
            var movies = await movieServices.GetMoviesByCriteriaAsync(moviesQuery);
            return Ok(movies);
        }
        [AllowAnonymous]
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