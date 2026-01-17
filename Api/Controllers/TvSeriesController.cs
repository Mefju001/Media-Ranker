using Application.Common.DTO.Request;
using Application.Features.TvSeriesServices.DeleteById;
using Application.Features.TvSeriesServices.GetAll;
using Application.Features.TvSeriesServices.GetTvSeriesByCriteria;
using Application.Features.TvSeriesServices.GetTvSeriesById;
using Application.Features.TvSeriesServices.TvSeriesUpsert;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TvSeriesController : ControllerBase
    {
        private readonly IMediator mediator;

        public TvSeriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllTvSeriesQuery();
            var movies = await mediator.Send(query);
            return Ok(movies);
        }
        [AllowAnonymous]
        [HttpGet("FilterBy")]
        public async Task<IActionResult> GetTvSeries([FromQuery] GetTvSeriesByCriteriaQuery tvSeriesQuery)
        {
            var movies = await mediator.Send(tvSeriesQuery);
            return Ok(movies);
        }
        [AllowAnonymous]
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetTvSeriesByIdQuery(id);
            var movie = await mediator.Send(query);
            return Ok(movie);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddTvSeries(TvSeriesRequest tvSeriesRequest)
        {
            var command = new UpsertTvSeriesCommand(null,
                tvSeriesRequest.title,
                tvSeriesRequest.description,
                tvSeriesRequest.genre,
                tvSeriesRequest.ReleaseDate,
                tvSeriesRequest.Language,
                tvSeriesRequest.Seasons,
                tvSeriesRequest.Episodes,
                tvSeriesRequest.Network,
                tvSeriesRequest.Status);
            var created = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = created.id }, created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateById/{id}")]
        public async Task<IActionResult> UpdateTvSeries(int id, TvSeriesRequest tvSeriesRequest)
        {
            var command = new UpsertTvSeriesCommand(id,
                tvSeriesRequest.title,
                tvSeriesRequest.description,
                tvSeriesRequest.genre,
                tvSeriesRequest.ReleaseDate,
                tvSeriesRequest.Language,
                tvSeriesRequest.Seasons,
                tvSeriesRequest.Episodes,
                tvSeriesRequest.Network,
                tvSeriesRequest.Status);
            var updated = await mediator.Send(command);
            return Ok(updated);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteByIdCommand(id);
            var deleted = await mediator.Send(command);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
