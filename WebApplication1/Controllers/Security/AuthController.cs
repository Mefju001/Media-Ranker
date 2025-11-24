using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers.Security
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;
        private readonly ITokenCleanupService tokenCleanupService;
        private readonly LogSenderService logSenderService;
        private readonly AppDbContext appDbContext;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly IGameServices game;
        public AuthController(LogSenderService logSenderService, AuthService authService, AppDbContext appDbContext, IPasswordHasher<User> password, ITokenCleanupService tokenCleanupService, IGameServices game)
        {
            this.logSenderService = logSenderService;
            this.authService = authService;
            this.appDbContext = appDbContext;
            this.passwordHasher = password;
            this.tokenCleanupService = tokenCleanupService;
            this.game = game;
        }

        [HttpPost("SendLogs")]
        public async Task<IActionResult> SendLogs()
        {
            //testing
            await logSenderService.SendLogAsync("Information", "Nothing", "admin");
            return Ok("Dane zostały przesłane. ");
        }
        [HttpPost("clean-tokens-now")]
        public async Task<IActionResult> CleanTokensNow()
        {
            await tokenCleanupService.Cleanup();
            return Ok("Czyszczenie tokenów rozpoczęte.");
        }
        [Authorize(Roles = ("User"),("Admin"))]
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
