using Application.Common.DTO.Request;
using Application.Common.Interfaces;
using Application.Features.AuthServices.Login;
using Application.Features.AuthServices.Logout;
using Application.Features.AuthServices.Signup;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUserContext currentUserContext;
        public AccountController(IMediator mediator, ICurrentUserContext currentUserContext)
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
                Expires = DateTime.Now.AddDays(1)
            });
            return Ok(new { Message = "Zalogowano pomyślnie. Przekazuje token dostępu: ", Token = tokens.accessToken });
        }
        [Authorize(Roles = "User")]
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = GetCurrentUserId();
            var jti = User.FindFirst("jti")?.Value;
            var command = new LogoutCommand(userId, jti);
            await mediator.Send(command);
            Response.Cookies.Delete("refreshToken");
            return Ok(new { Message = "wylogowano pomyślnie" });
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
        {
            var command = new SignUpCommand(
                userRequest.username,
                userRequest.email,
                userRequest.password,
                userRequest.name,
                userRequest.surname);
            var result = await mediator.Send(command);
            Response.Cookies.Append("refreshToken", result.refreshToken!, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.Now.AddDays(1)
            });
            return Ok(new { Message = $"Rejestracja użytkownika o nazwie {result.username} przebiegła pomyślnie. token dostępu: ", Token = result.accessToken });
        }
    }
}
