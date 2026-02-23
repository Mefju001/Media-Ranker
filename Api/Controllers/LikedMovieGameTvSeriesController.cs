using Application.Common.DTO.Request;
using Application.Common.DTO.Response;
using Application.Features.LikedServices.AddLiked;
using Application.Features.LikedServices.Delete;
using Application.Features.LikedServices.GetAllLiked;
using Application.Features.LikedServices.GetAllLikedByUser;
using Application.Features.LikedServices.GetByIdLiked;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("Api/Liked")]
    public class LikedMovieGameTvSeriesController : ControllerBase
    {
        private readonly IMediator mediator;
        private Guid? getUserId()
        {
            var stringUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (String.IsNullOrWhiteSpace(stringUserId)) return null;
            if (Guid.TryParse(stringUserId, out Guid userId))
            {
                return userId;
            }
            else
                return null;
        }
        public LikedMovieGameTvSeriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllQuery();
            var movies = await mediator.Send(query);
            return Ok(movies);
        }
        [Authorize(Roles = "User")]
        [HttpGet("getLikedByUser")]
        public async Task<IActionResult> GetLikedByUser()
        {
            var userId = getUserId();
            if (userId is null) return Unauthorized();
            var query = new GetAllLikedByUserQuery(userId.Value);
            return Ok(await mediator.Send(query));
        }
        [Authorize(Roles = "User")]
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
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(typeof(LikedMediaRequest), StatusCodes.Status201Created)]
        [HttpPost("Add")]
        public async Task<IActionResult> AddLiked([FromBody] LikedMediaRequest liked)
        {
            var userId = getUserId();
            if (userId is null) return Unauthorized();
            var command = new AddLikedCommand(userId.Value, liked.MediaId);
            var response = await mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = response.id }, response);
        }
        [Authorize(Roles = "Admin,User")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteLikedMovie([FromRoute] int id)
        {
            var userId = getUserId();
            if (userId is null) return Unauthorized();
            var command = new DeleteLikedCommand(userId.Value, id);
            await mediator.Send(command);
            return NoContent();
        }
    }
}
