using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Features.LikedServices.AddLiked;
using Application.Features.LikedServices.Delete;
using Application.Features.LikedServices.GetAllLiked;
using Application.Features.LikedServices.GetAllLikedByUser;
using Application.Features.LikedServices.GetByIdLiked;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController()]
    [Route("api/[controller]")]
    public class LikedController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUserContext currentUserContext;
        public LikedController(IMediator mediator, ICurrentUserContext currentUserContext)
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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllQuery();
            var movies = await mediator.Send(query);
            return Ok(movies);
        }
        [HttpGet("ForUser")]
        public async Task<IActionResult> GetLikedByUser()
        {
            var userId = GetCurrentUserId();
            var query = new GetAllLikedByUserQuery(userId);
            return Ok(await mediator.Send(query));
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(LikedMediaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var query = new GetByIdQuery(id);
            var response = await mediator.Send(query);
            if (response is null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [ProducesResponseType(typeof(LikedMediaRequest), StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<IActionResult> AddLiked([FromBody] LikedMediaRequest liked)
        {
            var userId = GetCurrentUserId();
            var command = new AddLikedCommand(userId, liked.MediaId);
            var response = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = response.id }, response);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLikedMovie([FromRoute] int id)
        {
            var userId = GetCurrentUserId();
            var command = new DeleteLikedCommand(userId, id);
            await mediator.Send(command);
            return NoContent();
        }
    }
}
