using Application.Features.AuthServices.CleanTokens;
using Application.Features.AuthServices.RefreshAccessToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("clean-tokens-now")]
        public async Task<IActionResult> CleanTokensNow()
        {
            var command = new CleanTokensCommand();
            await mediator.Send(command);
            return Ok("Czyszczenie tokenów rozpoczęte.");
        }
        [Authorize(Roles = ("User,Admin"))]
        [HttpPost("refreshToken")]
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
            return Ok();
        }
    }
}
