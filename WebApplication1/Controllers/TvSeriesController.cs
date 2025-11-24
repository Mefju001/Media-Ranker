using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO.Request;
using WebApplication1.QueryHandler.Query;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [Authorize]
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
        [HttpGet("FilterBy")]
        public async Task<IActionResult> GetTvSeries([FromQuery] TvSeriesQuery tvSeriesQuery)
        {
            var movies = await TvSeriesServices.GetMoviesByCriteriaAsync(tvSeriesQuery);
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
        public async Task<IActionResult> AddTvSeries(TvSeriesRequest tvSeriesRequest)
        {
            var created = await TvSeriesServices.Upsert(null, tvSeriesRequest);
            return CreatedAtAction(nameof(GetById), new { id = created.tvSeriesId }, created.response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateById/{id}")]
        public async Task<IActionResult> UpdateTvSeries(int id,TvSeriesRequest tvSeriesRequest)
        {
            var updated = await TvSeriesServices.Upsert(id,tvSeriesRequest);
            return Ok(updated);
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
