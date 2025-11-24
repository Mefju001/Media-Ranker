using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.DTO.Request;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserServices userServices;
        public UserController(IUserServices userServices)
        {
            this.userServices = userServices;
        }
        private string getStringUserId()
        {
            var stringUserId =  User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(stringUserId))
            {
                return "";
            }
            return stringUserId;
        }
        private int parse(string String)
        {
            if (int.TryParse(String, out int userId))
            {
                return userId;
            }
            else
                throw new ArgumentException("Nieprawidłowy format identyfikatora. Wymagana liczba całkowita.", nameof(String));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await userServices.GetAllAsync());
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            return Ok(await userServices.GetById(id));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("/{name}")]
        public async Task<IActionResult> GetBy(string name)
        {
            return Ok(await userServices.GetBy(name));
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPatch("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string newPassword, string confirmPassword, string oldPassword)
        {
            var stringUserId = getStringUserId();
            if (string.IsNullOrEmpty(stringUserId))
            {
                return Unauthorized();
            }
            int userId = parse(stringUserId);
            return Ok(await userServices.changePassword(newPassword, confirmPassword, oldPassword, userId));
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPatch("/ChangeDetails")]
        public async Task<IActionResult> ChangeDetails([FromBody]UserDetailsRequest userDetailsRequest)
        {
            var stringUserId = getStringUserId();
            if (string.IsNullOrEmpty(stringUserId))
            {
                return Unauthorized();
            }
            int userId = parse(stringUserId);
            return Ok(await userServices.changedetails(userId, userDetailsRequest));
        }
    }
}
