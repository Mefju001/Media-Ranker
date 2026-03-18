using Application.Features.AuthServices.CleanTokens;
using Application.Features.AuthServices.RefreshAccessToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;
        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("tokens/cleanup")]
        public async Task<IActionResult> CleanTokensNow()
        {
            var command = new CleanTokensCommand();
            await mediator.Send(command);
            return NoContent();
        }
        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken is null)
            {
                return Unauthorized();
            }
            var command = new RefreshAccessTokenCommand(refreshToken);
            var tokens = await mediator.Send(command);
            if (tokens is null)
            {
                throw new Exception("Not found tokens to use");
            }
            Response.Cookies.Append("refreshToken", tokens.refreshToken, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.Now.AddDays(7)
            });
            return Ok(new {AccessToken = tokens.accessToken});
        }
    }
}
