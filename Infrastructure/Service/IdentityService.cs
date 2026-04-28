using Application.Common.DTO;
using Application.Common.Interfaces;
using Domain.Exceptions;
using Infrastructure.Database.DBModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Service
{
    public class IdentityService: IIdentityService
    {
        private readonly UserManager<UserModel> userManager;
        public IdentityService(UserManager<UserModel> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<UserDTO> CreateUserWithDefaultRole(string username, string password, string email)
        {
            var identityUser = new UserModel
            {
                Id = Guid.NewGuid(),
                UserName = username,
                Email = email,
            };
            var result = await userManager.CreateAsync(identityUser, password);
            if (!result.Succeeded) throw new Exception("User creation failed: " + result.Errors.Select(e => e.Description));
            var defaultRoles = new List<string> { "User" };
            await userManager.AddToRolesAsync(identityUser, defaultRoles);
            return new UserDTO(identityUser.Id, identityUser.UserName, identityUser.Email, defaultRoles);
        }
        public async Task<bool> IsAnyUserWhoHaveEmailAndId(string email, string username, CancellationToken cancellationToken)
        {
            return await userManager.Users.AnyAsync(u => u.Email == email && u.UserName != username , cancellationToken);
        }
        public async Task<UserDTO?> AuthenticateAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return null;
            var result = await userManager.CheckPasswordAsync(user, password);
            if (result == false) return null;
            var roles = await userManager.GetRolesAsync(user);
            return new UserDTO(user.Id, user.UserName, user.Email, roles.ToList());
        }

        public async Task<UserDTO> GetUserById(Guid userId, CancellationToken cancellationToken)
        {
            var userModel = await userManager.Users
                    .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

            if (userModel == null) return null;

            var roles = await userManager.GetRolesAsync(userModel);

            return new UserDTO(userModel.Id, userModel.UserName, userModel.Email, roles.ToList());
        }
        public async Task ChangePassword(Guid userId, string currentPassword, string newPassword)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new NotFoundException("User not found");
            var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!result.Succeeded) throw new InvalidOperationException("Password change failed");
        }
        public async Task DeleteUser(Guid id)
        {
            var userModel = await userManager.FindByIdAsync(id.ToString());
            if (userModel == null)
            {
                throw new NotFoundException("User not found in store.");
            }
            var result = await userManager.DeleteAsync(userModel);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException("Failed to delete user.");
            }
        }
    }
}
