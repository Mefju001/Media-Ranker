using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO.Request;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameServices gameServices;

        public GameController(IGameServices gameServices)
        {
            this.gameServices = gameServices;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var games = await gameServices.GetAllAsync();
            return Ok(games);
        }
        [HttpGet("sortBy/{sort}")]
        public async Task<IActionResult> GetSortAll(string sort)
        {
            var games = await gameServices.GetSortAll(sort);
            return Ok(games);
        }
        [HttpGet("FilterBy")]
        public async Task<IActionResult> GetMovies([FromQuery] string? name, [FromQuery] string? genreName)
        {
            var games = await gameServices.GetGames(name, genreName);
            return Ok(games);
        }
        [HttpGet("byAvarage")]
        public async Task<IActionResult> GetMoviesByAvarage()
        {
            var games = await gameServices.GetGamesByAvrRating();
            return Ok(games);
        }
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var games = await gameServices.GetById(id);
            return Ok(games);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Upsert(int? id, GameRequest gameRequest)
        {
            var created = await gameServices.Upsert(id, gameRequest);
            if (id is null)
                return CreatedAtAction(nameof(GetById), new { id = created.movieId }, created.response);
            return Ok(created.response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await gameServices.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
