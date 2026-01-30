using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        /*private readonly IUserServices userServices;
        public UserController(IUserServices userServices)
        {
            this.userServices = userServices;
        }
        private int? getUserId()
        {
            var stringUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(stringUserId, out int userId))
            {
                return userId;
            }
            else
                return null;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await userServices.GetAllAsync());
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("id:{int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            return Ok(await userServices.GetById(id));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("by-name/{name}")]
        public async Task<IActionResult> GetBy(string name)
        {
            return Ok(await userServices.GetBy(name));
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPatch("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string newPassword, string confirmPassword, string oldPassword)
        {
            var userId = getUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            await userServices.changePassword(newPassword, confirmPassword, oldPassword, userId.Value);
            return Ok();
        }
        [Authorize(Roles = "Admin,User")]
        [HttpPatch("ChangeDetails")]
        public async Task<IActionResult> ChangeDetails([FromBody] UserDetailsRequest userDetailsRequest)
        {
            var userId = getUserId();
            if (userId == null)
            {
                return Unauthorized();
            }
            await userServices.changedetails(userId.Value, userDetailsRequest);
            return Ok();
        }*/
    }
}
