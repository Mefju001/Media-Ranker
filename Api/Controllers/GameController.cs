using Application.Common.DTO.Request;
using Application.Features.GamesServices.AddListOfGames;
using Application.Features.GamesServices.DeleteById;
using Application.Features.GamesServices.GameUpsert;
using Application.Features.GamesServices.GetGameById;
using Application.Features.GamesServices.GetGamesByCriteria;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IMediator mediator;
        public GameController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetGamesByCriteriaQuery gameQuery)
        {
            var games = await mediator.Send(gameQuery);
            return Ok(games);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var query = new GetGameByIdQuery(id);
            var games = await mediator.Send(query);
            return Ok(games);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddGame([FromBody] GameRequest gameRequest)
        {
            var command = new UpsertGameCommand(null,
                gameRequest.Title,
                gameRequest.Description,
                gameRequest.Genre,
                gameRequest.ReleaseDate,
                gameRequest.Language,
                gameRequest.Developer,
                gameRequest.Platform);
            var created = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Bulk")]
        public async Task<IActionResult> AddListOfGames([FromBody] List<GameRequest> gameRequests)
        {
            var command = new AddListOfGamesCommand(gameRequests);
            var createdGames = await mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, createdGames);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateGame([FromRoute] int id, [FromBody] GameRequest gameRequest)
        {
            var command = new UpsertGameCommand(id,
                gameRequest.Title,
                gameRequest.Description,
                gameRequest.Genre,
                gameRequest.ReleaseDate,
                gameRequest.Language,
                gameRequest.Developer,
                gameRequest.Platform);
            var updated = await mediator.Send(command);
            return Ok(updated);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteByIdCommand(id);
            var deleted = await mediator.Send(command);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
