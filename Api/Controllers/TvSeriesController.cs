using Application.Features.Genres.GetAllForChooseMedias;
using Application.Features.Medias.TvSeries.AddRange;
using Application.Features.Medias.TvSeries.Common;
using Application.Features.Medias.TvSeries.DeleteById;
using Application.Features.Medias.TvSeries.GetByCriteria;
using Application.Features.Medias.TvSeries.GetById;
using Application.Features.Medias.TvSeries.Upsert;
using Domain.Aggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TvSeriesController : ControllerBase
    {
        private readonly IMediator mediator;

        public TvSeriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetByCriteriaQuery tvSeriesQuery, CancellationToken cancellationToken)
        {
            var movies = await mediator.Send(tvSeriesQuery, cancellationToken);
            return Ok(movies);
        }
        [AllowAnonymous]
        [HttpGet("Genres")]
        public async Task<IActionResult> GetGenres(CancellationToken cancellationToken)
        {
            var query = new GetUsedForMediaQuery<TvSeries>();
            var result = await mediator.Send(query, cancellationToken);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var query = new GetByIdQuery(id);
            var movie = await mediator.Send(query, cancellationToken);
            return Ok(movie);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddTvSeries(TvSeriesRequest tvSeriesRequest, CancellationToken cancellationToken)
        {
            var command = new UpsertCommand(null,
                tvSeriesRequest.title,
                tvSeriesRequest.description,
                tvSeriesRequest.genre,
                tvSeriesRequest.ReleaseDate,
                tvSeriesRequest.Language,
                tvSeriesRequest.Seasons,
                tvSeriesRequest.Episodes,
                tvSeriesRequest.Network,
                tvSeriesRequest.Status);
            var created = await mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Bulk")]
        public async Task<IActionResult> AddListOfSeries(List<TvSeriesRequest> tvSeriesRequests, CancellationToken cancellationToken)
        {
            var command = new AddRangeCommand(tvSeriesRequests);
            var created = await mediator.Send(command, cancellationToken);
            return Ok(created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTvSeries([FromRoute] Guid id, TvSeriesRequest tvSeriesRequest, CancellationToken cancellationToken)
        {
            var command = new UpsertCommand(id,
                tvSeriesRequest.title,
                tvSeriesRequest.description,
                tvSeriesRequest.genre,
                tvSeriesRequest.ReleaseDate,
                tvSeriesRequest.Language,
                tvSeriesRequest.Seasons,
                tvSeriesRequest.Episodes,
                tvSeriesRequest.Network,
                tvSeriesRequest.Status);
            var updated = await mediator.Send(command, cancellationToken);
            return Ok(updated);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteByIdCommand(id);
            var deleted = await mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}
