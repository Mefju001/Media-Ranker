
using Application.Features.Genres.GetAllForMedias;
using Application.Features.Movies.AddRange;
using Application.Features.Movies.Common;
using Application.Features.Movies.DeleteById;
using Application.Features.Movies.GetByCriteria;
using Application.Features.Movies.GetMovieById;
using Application.Features.Movies.Upsert;
using Domain.Aggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMediator mediator;

        public MovieController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetByCriteriaQuery moviesQuery, CancellationToken cancellationToken)
        {
            var movies = await mediator.Send(moviesQuery, cancellationToken);
            return Ok(movies);
        }
        [AllowAnonymous]
        [HttpGet("Genres")]
        public async Task<IActionResult> GetGenres(CancellationToken cancellationToken)
        {
            var query = new GetUsedForMediaQuery<Movie>();
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var query = new GetByIdQuery(id);
            var result = await mediator.Send(query, cancellationToken);
            if (result is null) return NotFound();
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody] MovieRequest movie, CancellationToken cancellationToken)
        {
            var command = new UpsertCommand(
                null,
                movie.Title,
                movie.Description,
                movie.Genre,
                movie.Director,
                movie.ReleaseDate,
                movie.Language,
                movie.Duration,
                movie.DistributionType,
                movie.Status);
            var created = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Bulk")]
        public async Task<IActionResult> AddMovies([FromBody] List<MovieRequest> movies, CancellationToken cancellationToken)
        {
            var command = new AddRangeCommand(movies);
            var created = await mediator.Send(command, cancellationToken);
            return Ok(created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateMovie([FromRoute] Guid id, [FromBody] MovieRequest movie, CancellationToken cancellationToken)
        {
            var command = new UpsertCommand(
                id,
                movie.Title,
                movie.Description,
                movie.Genre,
                movie.Director,
                movie.ReleaseDate,
                movie.Language,
                movie.Duration,
                movie.DistributionType,
                movie.Status);
            var updated = await mediator.Send(command, cancellationToken);
            return Ok(updated);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var deleted = await mediator.Send(new DeleteByIdCommand(id), cancellationToken);
            return NoContent();
        }

    }
}