using Application.Features.UserInteractions.Rankings.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RankingController : ControllerBase
    {
        private readonly IMediator mediator;
        public RankingController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> GetRatings(string mediaType)
        {
            var query = new GetQuery(mediaType);
            var ratings = await mediator.Send(query);
            return Ok(ratings);
        }
    }
}
