using Application.Common.DTO;
using Application.Common.Interfaces;
using Infrastructure.Database.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Database.Repository
{

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<UserModel> userManager;

        public UserRepository(AppDbContext appDbContext, UserManager<UserModel> userManager)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
        }
        public async Task<IdentityResult> ChangePassword(Guid userId, string currentPassword, string newPassword)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            return await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }
        public async Task<UserDTO?> AuthenticateAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return null;
            var result = await userManager.CheckPasswordAsync(user, password);
            if (result == false) return null;
            var roles = await userManager.GetRolesAsync(user);
            return new UserDTO(user.Id,user.UserName,user.Email,roles.ToList());
        }
        public async Task<UserDTO> GetUserById(Guid userId, CancellationToken cancellationToken)
        {
            var userModel = await appDbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (userModel == null) return null;

            var roles = await userManager.GetRolesAsync(userModel);

            return new UserDTO(userModel.Id, userModel.UserName, userModel.Email, roles.ToList());
        }
        public async Task<bool> IsAnyUserWhoHaveEmailAndId(string email, Guid id)
        {
            return await appDbContext.Users.AnyAsync(u => u.Email == email && u.Id != id);
        }
        public async Task<IdentityResult?> DeleteUser(Guid id)
        {
            var userModel = await userManager.FindByIdAsync(id.ToString());

            if (userModel == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found in store." });
            }
            var result = await userManager.DeleteAsync(userModel);
            return result;
        }
        public async Task<bool> IsAnyUserWithUsernameAndEmailLikeThat(string username, string email)
        {
            return await appDbContext.Users.AnyAsync(u => u.UserName == username || u.Email == email);
        }
        public async Task<UserDTO> CreateUserWithDefaultRole(string username, string password, string email, CancellationToken cancellationToken)
        {
            var identityUser = new UserModel
            {
                Id = Guid.NewGuid(),
                UserName = username,
                Email = email,
            };
            var result = await userManager.CreateAsync(identityUser,password);
            if (!result.Succeeded) throw new Exception("User creation failed: " + result.Errors.Select(e => e.Description));
            var defaultRoles = new List<string> { "User" };
            await userManager.AddToRolesAsync(identityUser, defaultRoles);
            return new UserDTO(identityUser.Id,identityUser.UserName, identityUser.Email, defaultRoles);
        }

        public async Task<string> GetUsernameById(Guid id, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;
            return user.UserName!;
        }
    }
}
