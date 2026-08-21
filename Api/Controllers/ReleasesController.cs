using Application.Features.Medias.PremieresAndAnnouncements.GetReleases;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReleasesController : ControllerBase
    {
        private readonly IMediator mediator;
        public ReleasesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetReleases(string scope, string mediaType)
        {
            var recommendationQuery = new GetReleasesQuery(scope, mediaType);
            var recommedations = await mediator.Send(recommendationQuery);
            return Ok(recommedations);
        }
    }
}
