using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO.Request;
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
            return Ok(new { Token = token });
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
        {
            return Ok(await userServices.Register(userRequest));
        }
    }
}
