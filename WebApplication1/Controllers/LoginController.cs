using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO.Request;
using WebApplication1.Migrations;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api")]
    public class LoginController:ControllerBase
    {
        private readonly IUserServices userServices;
        private readonly AuthService authService;

        public LoginController(AuthService authService, IUserServices userServices)
        {
            this.authService = authService;
            this.userServices = userServices;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var token = await authService.Login(loginRequest);
            if (token == null)
            {
                return Unauthorized("Nieprawidłowe dane");
            }
            /*Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.Now.AddDays(7)
            });
            if (refreshToken is null)
            {
                throw new Exception("Refresh token is null");
            }*/
            return Ok(new { Message = "Zalogowano pomyślnie.", Token = token.token });

        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
        {
            return Ok(await userServices.Register(userRequest));
        }

        [AllowAnonymous]
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if(refreshToken is null)
            {
                return Unauthorized();
            }
            await authService.RefreshAccessToken(refreshToken);
            return Ok();
        }
    }
}
