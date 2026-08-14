using Application.Features.PremieresAndAnnouncements.GetReleases;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PremieresAndAnnouncementsController: ControllerBase
    {
        private readonly IMediator mediator;
        public PremieresAndAnnouncementsController(IMediator mediator)
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
