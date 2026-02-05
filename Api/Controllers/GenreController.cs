using Application.Common.Interfaces;
using Application.Features.GenreServices;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly IMediator mediator;
        public GenreController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetGenres()
        {
            var query = new GetGenresQuery();
            var results = await mediator.Send(query);
            return Ok(results);
        }
    }
}
