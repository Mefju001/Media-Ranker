using Application.Common.DTO.Request;
using Application.Features.GamesServices.AddListOfGames;
using Application.Features.GamesServices.DeleteById;
using Application.Features.GamesServices.GameUpsert;
using Application.Features.GamesServices.GetAll;
using Application.Features.GamesServices.GetGameById;
using Application.Features.GamesServices.GetGamesByCriteria;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
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
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllQuery();
            var games = await mediator.Send(query);
            return Ok(games);
        }
        [AllowAnonymous]
        [HttpGet("FilterBy")]
        public async Task<IActionResult> GetGames([FromQuery] GetGamesByCriteriaQuery gameQuery)
        {
            var games = await mediator.Send(gameQuery);
            return Ok(games);
        }
        [HttpGet("FindById/{id}")]
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
        [HttpPost]
        public async Task<IActionResult> AddListOfGames([FromBody] List<GameRequest> gameRequests)
        {
            var command = new AddListOfGamesCommand(gameRequests);
            var createdGames = await mediator.Send(command);
            return StatusCode(StatusCodes.Status201Created, createdGames);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateGameById/{id}")]
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
        [HttpDelete("DeleteById/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteByIdCommand(id);
            var deleted = await mediator.Send(command);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
