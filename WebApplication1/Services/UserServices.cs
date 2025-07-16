using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Data;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class UserServices : IUserServices
    {
        private readonly IPasswordHasher<User> Hasher;
        private AppDbContext dbContext;
        public UserServices(AppDbContext dbContext, IPasswordHasher<User> passwordHasher)
        {
            this.dbContext = dbContext;
            Hasher = passwordHasher;
        }
        public async Task<bool> changePassword(string newPassword, string confirmPassword, string oldPassword, int userId)
        {
            if (!string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(confirmPassword) && !string.IsNullOrEmpty(oldPassword))
            {
                var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    var passwordVerification = Hasher.VerifyHashedPassword(user, user.password, oldPassword);
                    if (string.Equals(newPassword,confirmPassword,StringComparison.Ordinal) && passwordVerification.Equals(PasswordVerificationResult.Success)&&!string.Equals(oldPassword, newPassword,StringComparison.Ordinal))
                    {
                        user.password = Hasher.HashPassword(user, newPassword);
                        await dbContext.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        public async Task<bool>changedetails(int userId, UserDetailsRequest userDetailsRequest)
        {
            var emailExists = await dbContext.Users.AnyAsync(u=>u.email == userDetailsRequest.email&&u.Id!=userId);
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user is null || emailExists)
                return false;
            user.setUser(userDetailsRequest);
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Delete(int id)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null) return false;
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<UserResponse>> GetAllAsync()
        {
            var users = await dbContext.Users
                .Include(u=>u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Include(u=>u.Reviews)
                .ThenInclude(r=>r.Media)
                .ToListAsync();
            return users.Select(UserMapping.ToResponse).ToList();
        }

        public async Task<bool> Register(UserRequest userRequest)
        {
            bool exists = await dbContext.Users.AnyAsync(u => u.username == userRequest.username || u.email == userRequest.email);
            if (exists)
                return false;
            var user = new User
            {
                username = userRequest.username,
                password = Hasher.HashPassword(null,userRequest.password),
                name = userRequest.name,
                surname = userRequest.surname,
                email = userRequest.email,
            };
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            var role = await dbContext.Roles.FirstOrDefaultAsync(r=>r.role == ERole.User);
            if (role is null) return false;
            var UserRoles = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id
            };
            dbContext.UsersRoles.Add(UserRoles);
            await dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<UserResponse?> GetById(int id)
        {
            var user = await dbContext.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Reviews)
                    .ThenInclude(r => r.Media)
                .FirstOrDefaultAsync(u=>u.Id == id);
            if (user is null) return null;
            return UserMapping.ToResponse(user);
        }
        public async Task<List<UserResponse>>GetBy(string name)
        {
            var User = await dbContext.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.Reviews)
                    .ThenInclude(r => r.Media)
                .Where(u=>
                        EF.Functions.Like(u.name,$"%{name}") ||
                        EF.Functions.Like(u.surname,$"{name}")||
                        u.username == name ||
                        u.email == name)
                .ToListAsync();
            if (!User.Any()) return new List<UserResponse>();
            return User.Select(UserMapping.ToResponse).ToList();
        }
    }
}
