using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO.Request;
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
        [HttpGet("FilterBy")]
        public async Task<IActionResult> GetMovies([FromQuery] MovieQuery moviesQuery)
        {
            var movies = await movieServices.GetMoviesByCriteriaAsync(moviesQuery);
            return Ok(movies);
        }
        [AllowAnonymous]
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var movie = await movieServices.GetById(id);
            return Ok(movie);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody]MovieRequest movie)
        {
            var created = await movieServices.Upsert(null, movie);
            return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("updateById/{id}")]
        public async Task<IActionResult> UpdateMovie([FromRoute]int id, [FromBody] MovieRequest movie)
        {
            var updated = await movieServices.Upsert(id, movie);
            return Ok(updated);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("deleteById/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deleted = await movieServices.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

    }
}