using Application.Features.Genres.GetGenres;
using Application.Features.Medias.Games.AddRange;
using Application.Features.Medias.Games.Common;
using Application.Features.Medias.Games.DeleteById;
using Application.Features.Medias.Games.GetByCriteria;
using Application.Features.Medias.Games.GetById;
using Application.Features.Medias.Games.GetPlatforms;
using Application.Features.Medias.Games.Upsert;
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
        public async Task<IActionResult> Get([FromQuery] GetByCriteriaQuery gameQuery, CancellationToken cancellationToken)
        {
            var games = await mediator.Send(gameQuery, cancellationToken);
            return Ok(games);
        }
        [AllowAnonymous]
        [HttpGet("Genres")]
        public async Task<IActionResult> GetGenres(CancellationToken cancellationToken)
        {
            var query = new GetGenresQuery(EMediaType.Game);
            var games = await mediator.Send(query, cancellationToken);
            return Ok(games);
        }
        [AllowAnonymous]
        [HttpGet("Platforms")]
        public async Task<IActionResult> GetPlatforms(CancellationToken cancellationToken)
        {
            var query = new GetPlatformsQuery();
            var games = await mediator.Send(query, cancellationToken);
            return Ok(games);
        }
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var query = new GetByIdQuery(id);
            var games = await mediator.Send(query, cancellationToken);
            return Ok(games);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddGame([FromBody] GameRequest gameRequest, CancellationToken cancellationToken)
        {
            var command = new UpsertCommand(
                null,
                gameRequest.Title,
                gameRequest.Description,
                gameRequest.Genre,
                gameRequest.ReleaseDate,
                gameRequest.Language,
                gameRequest.GameStatus,
                gameRequest.Developer,
                gameRequest.Engine,
                gameRequest.PegiRating,
                gameRequest.Platforms,
                gameRequest.SupportsCrossPlay
                );
            var created = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { Id = created.id }, created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Bulk")]
        public async Task<IActionResult> AddListOfGames([FromBody] List<GameRequest> gameRequests, CancellationToken cancellationToken)
        {
            var command = new AddRangeCommand(gameRequests);
            var createdGames = await mediator.Send(command, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, createdGames);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateGame([FromRoute] Guid id, [FromBody] GameRequest gameRequest, CancellationToken cancellationToken)
        {
            var command = new UpsertCommand(
                id,
                gameRequest.Title,
                gameRequest.Description,
                gameRequest.Genre,
                gameRequest.ReleaseDate,
                gameRequest.Language,
                gameRequest.GameStatus,
                gameRequest.Developer,
                gameRequest.Engine,
                gameRequest.PegiRating,
                gameRequest.Platforms,
                gameRequest.SupportsCrossPlay);
            var updated = await mediator.Send(command, cancellationToken);
            return Ok(updated);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteByIdCommand(id);
            await mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
