using Application.Features.Common.Interfaces;
using Application.Features.Recommendation.GetForUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationController: ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUserContext currentUserContext;
        public RecommendationController(IMediator mediator, ICurrentUserContext currentUserContext)
        {
            this.mediator = mediator;
            this.currentUserContext = currentUserContext;
        }
        private Guid GetCurrentUserId()
        {
            var userId = currentUserContext.UserId;
            if (userId is null) throw new UnauthorizedAccessException();
            return userId.Value;
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var recommendationQuery = new GetForUserQuery(GetCurrentUserId());
            var recommedations = await mediator.Send(recommendationQuery);
            return Ok(recommedations);
        }
    }
}
