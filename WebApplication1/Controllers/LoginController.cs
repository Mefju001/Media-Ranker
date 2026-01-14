using Application.Features.AuthServices.Login;
using Application.Features.AuthServices.Logout;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Application.Common.DTO.Request;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class LoginController : ControllerBase
    {
        private readonly IMediator mediator;

        public LoginController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var command = new LoginCommand(loginRequest.username, loginRequest.password);
            var tokens = await mediator.Send(command);
            if (tokens == null)
            {
                return Unauthorized("Nieprawidłowe dane");
            }
            Response.Cookies.Append("refreshToken", tokens.refreshToken, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.Now.AddDays(7)
            });
            return Ok(new { Message = "Zalogowano pomyślnie. Przekazuje token dostępu: ", Token = tokens.accessToken });
        }
        [Authorize(Roles = "User")]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Nie znaleziono ID użytkownika w tokenie.");
            }
            var command = new LogoutCommand(userId);
            await mediator.Send(command);
            Response.Cookies.Delete("refreshToken");
            return Ok(new { Message = "wylogowano pomyślnie" });
        }
        /*[AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
        {
            return Ok(await userServices.Register(userRequest));
        }*/
    }
}
