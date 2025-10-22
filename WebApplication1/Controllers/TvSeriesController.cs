using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO.Request;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TvSeriesController : ControllerBase
    {
        private readonly ITvSeriesServices TvSeriesServices;

        public TvSeriesController(ITvSeriesServices tvSeriesServices)
        {
            TvSeriesServices = tvSeriesServices;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movies = await TvSeriesServices.GetAllAsync();
            return Ok(movies);
        }
        [AllowAnonymous]
        [HttpGet("sortBy/{sort}")]
        public async Task<IActionResult> GetSortAll(string sort)
        {
            var movies = await TvSeriesServices.GetSortAll(sort);
            return Ok(movies);
        }
        [AllowAnonymous]
        [HttpGet("FilterBy")]
        public async Task<IActionResult> GetMovies([FromQuery] string? name, [FromQuery] string? genreName, [FromQuery] string? directorName)
        {
            var movies = await TvSeriesServices.GetTvSeries(name, genreName, directorName);
            return Ok(movies);
        }
        [AllowAnonymous]
        [HttpGet("byAvarage")]
        public async Task<IActionResult> GetMoviesByAvarage()
        {
            var movies = await TvSeriesServices.GetTvSeriesByAvrRating();
            return Ok(movies);
        }
        [AllowAnonymous]
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await TvSeriesServices.GetById(id);
            return Ok(movie);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Upsert(int? id, TvSeriesRequest tvSeriesRequest)
        {
            var created = await TvSeriesServices.Upsert(id, tvSeriesRequest);
            if (id is null)
                return CreatedAtAction(nameof(GetById), new { id = created.tvSeriesId }, created.response);
            return Ok(created.response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await TvSeriesServices.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
