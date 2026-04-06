using Application.Common.DTO;
using Application.Common.Interfaces;
using Domain.Aggregate;
using Domain.Enums;
using Infrastructure.Database.DBModels;
using Infrastructure.Database.DBModels.Extensions;
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
        /*public async Task<UserDetails> GetUserByUsername(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return null;
            var roles = await userManager.GetRolesAsync(user);
            var domainRoles = roles.Select(r =>
                Enum.TryParse<ERole>(r, out var role) ? (ERole?)role : null).OfType<ERole>().ToList();
            return user.ToDomain(domainRoles);
        }*/
        public async Task<UserDTO?> AuthenticateAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return null;
            var result = await userManager.CheckPasswordAsync(user, password);
            if (result == false) return null;
            var roles = await userManager.GetRolesAsync(user);
            return new UserDTO(user.Id,user.UserName,user.Email,roles.ToList());
        }
        public async Task<IList<string>> getUserRoles(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return null;
            var results = await userManager.GetRolesAsync(user);
            return results;
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
/*        public async Task<Dictionary<Guid, UserDetails>> GetByIds(List<Guid> userIds, CancellationToken cancellationToken)
        {
            var identityUsers = await userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync(cancellationToken);
            var userDictionary = new Dictionary<Guid, UserDetails>();
            foreach (var user in identityUsers)
            {
                var roles = await userManager.GetRolesAsync(user);
                var domainRoles = roles
                    .Select(r => Enum.TryParse<ERole>(r, out var role) ? (ERole?)role : null)
                    .OfType<ERole>()
                    .ToList();
                userDictionary[user.Id] = user.ToDomain(domainRoles);
            }
            return userDictionary;
        }*/

        public async Task<string> GetUsernameById(Guid id, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;
            return user.UserName!;
        }
        public async Task<Dictionary<Guid, string>> GetUsernamesByIds(List<Guid> ids, CancellationToken cancellationToken)
        {
            var users = await userManager.Users.Where(u => ids.Contains(u.Id)).ToDictionaryAsync(u => u.Id, u => u.UserName, cancellationToken);
            return users!;
        }
    }
}
