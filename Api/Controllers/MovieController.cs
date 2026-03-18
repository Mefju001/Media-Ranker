using Application.Common.DTO.Request;
using Application.Features.MovieServices.AddListOfMovies;
using Application.Features.MovieServices.DeleteById;
using Application.Features.MovieServices.GetAll;
using Application.Features.MovieServices.GetMovieById;
using Application.Features.MovieServices.GetMoviesByCriteria;
using Application.Features.MovieServices.MovieUpsert;
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
        public async Task<IActionResult> Get([FromQuery] GetMoviesByCriteriaQuery moviesQuery)
        {
            var movies = await mediator.Send(moviesQuery);
            return Ok(movies);
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var query = new GetMovieByIdQuery(id);
            var result = await mediator.Send(query);
            if (result is null) return NotFound();
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody] MovieRequest movie)
        {
            var command = new UpsertMovieCommand(
                null,
                movie.Title,
                movie.Description,
                movie.Genre,
                movie.Director,
                movie.ReleaseDate,
                movie.Language,
                movie.Duration,
                movie.IsCinemaRelease);
            var created = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Bulk")]
        public async Task<IActionResult> AddMovies([FromBody] List<MovieRequest> movies)
        {
            var command = new AddListOfMoviesCommand(movies);
            var created = await mediator.Send(command);
            return Ok(created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateMovie([FromRoute] int id, [FromBody] MovieRequest movie)
        {
            var command = new UpsertMovieCommand(
                id,
                movie.Title,
                movie.Description,
                movie.Genre,
                movie.Director,
                movie.ReleaseDate,
                movie.Language,
                movie.Duration,
                movie.IsCinemaRelease);
            var updated = await mediator.Send(command);
            return Ok(updated);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deleted = await mediator.Send(new DeleteByIdCommand(id));
            return NoContent();
        }

    }
}