using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.DTO.Request;
using WebApplication1.Interfaces;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api")]
    public class LoginController : ControllerBase
    {
        private readonly IUserServices userServices;
        private readonly AuthService authService;

        public LoginController(AuthService authService, IUserServices userServices)
        {
            this.authService = authService;
            this.userServices = userServices;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var tokens = await authService.Login(loginRequest);
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
            await authService.Logout(userId);
            Response.Cookies.Delete("refreshToken");
            return Ok(new { Message = "wylogowano pomyślnie" });
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
        {
            return Ok(await userServices.Register(userRequest));
        }

        [Authorize(Roles = ("User"))]
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (refreshToken is null)
            {
                return Unauthorized();
            }
            var tokens = await authService.RefreshAccessToken(refreshToken);
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
