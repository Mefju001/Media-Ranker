using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO.Request;
using WebApplication1.QueryHandler.Query;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameServices gameServices;

        public GameController(IGameServices gameServices)
        {
            this.gameServices = gameServices;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var games = await gameServices.GetAllAsync();
            return Ok(games);
        }
        [AllowAnonymous]
        [HttpGet("FilterBy")]
        public async Task<IActionResult> GetGames([FromQuery] GameQuery gameQuery)
        {
            var games = await gameServices.GetGamesByCriteriaAsync(gameQuery);
            return Ok(games);
        }
        [HttpGet("FindById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var games = await gameServices.GetById(id);
            return Ok(games);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddGame(GameRequest gameRequest)
        {
            var created = await gameServices.Upsert(null, gameRequest);
            return CreatedAtAction(nameof(GetById), new { id = created.id },created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateGameById/{id}")]
        public async Task<IActionResult> UpdateGame(int id, GameRequest gameRequest)
        {
            var updated = await gameServices.Upsert(id, gameRequest);
            return Ok(updated);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await gameServices.Delete(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
