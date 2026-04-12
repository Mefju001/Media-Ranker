using Application.Common.DTO.Request;
using Application.Features.TvSeriesServices.AddListOfTvSeries;
using Application.Features.TvSeriesServices.DeleteById;
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
        public async Task<IActionResult> Get([FromQuery] GetTvSeriesByCriteriaQuery tvSeriesQuery)
        {
            var movies = await mediator.Send(tvSeriesQuery);
            return Ok(movies);
        }
        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
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
        [HttpPost("Bulk")]
        public async Task<IActionResult> AddListOfSeries(List<TvSeriesRequest> tvSeriesRequests)
        {
            var command = new AddListOfTvSeriesCommand(tvSeriesRequests);
            var created = await mediator.Send(command);
            return Ok(created);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTvSeries([FromRoute] int id, TvSeriesRequest tvSeriesRequest)
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
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var command = new DeleteByIdCommand(id);
            var deleted = await mediator.Send(command);
            return NoContent();
        }
    }
}
