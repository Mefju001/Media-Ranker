using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Controllers.Security
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly AuthService authService;
        private readonly ITokenCleanupService tokenCleanupService;
        private readonly AppDbContext appDbContext;
        private readonly IPasswordHasher<User>passwordHasher;
        public AuthController(AuthService authService, AppDbContext appDbContext, IPasswordHasher<User> password, ITokenCleanupService tokenCleanupService)
        {
            this.authService = authService;
            this.appDbContext = appDbContext;
            this.passwordHasher = password;
            this.tokenCleanupService = tokenCleanupService;
        }

        [HttpPost("AddRolesAndUsers")]
        public async Task<IActionResult> AddUserAndRole()
        {
            var User = new User { username = "Mati",password = passwordHasher.HashPassword(null,"Starwars2"),name = "Mateusz",surname = "Jureczko",email = "jureczkomateusz@wp.pl"};
            var Role = new Role { role = ERole.User };

            appDbContext.Users.Add(User);
            appDbContext.Roles.Add(Role);
            await appDbContext.SaveChangesAsync();
            var UserRole = new UserRole
            {
                UserId = User.Id,
                RoleId = Role.Id
            };
            appDbContext.UsersRoles.Add(UserRole);
            await appDbContext.SaveChangesAsync();
            return Ok("Dane zostały dodane.");
        }
        [HttpPost("role")]
        public async Task<IActionResult> ChangeRole()
        {
            User? user = await appDbContext.Users.FirstOrDefaultAsync(u => u.username == "Mefju");
            Role? role = appDbContext.Roles.FirstOrDefault(r => r.role.Equals(ERole.User));
            if (user == null && role == null) { return this.NoContent(); }
            var UserRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };
            appDbContext.UsersRoles.Add(UserRole);
            await appDbContext.SaveChangesAsync();

            return Ok("Dane zostały dodane.");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("clean-tokens-now")]
        public async Task<IActionResult> CleanTokensNow()
        {
            await tokenCleanupService.Cleanup();
            return Ok("Czyszczenie tokenów rozpoczęte.");
        }
    }
}
