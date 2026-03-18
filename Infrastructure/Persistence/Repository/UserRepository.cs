using Application.Common.Interfaces;
using Domain.Entity;
using Domain.Enums;
using Domain.Exceptions;
using Infrastructure.DBModels;
using Infrastructure.DBModels.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Persistence.Repository
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
        public async Task<User> GetUserByUsername(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return null;
            var roles = await userManager.GetRolesAsync(user);
            var domainRoles = roles.Select(r =>
                Enum.TryParse<ERole>(r, out var role) ? (ERole?)role : null).OfType<ERole>().ToList();
            return user.ToDomain(domainRoles);
        }
        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return null;
            var result = await userManager.CheckPasswordAsync(user, password);
            if (result == false) return null;
            var roles = await userManager.GetRolesAsync(user);
            var domainRoles = roles.Select(r =>
                Enum.TryParse<ERole>(r, out var role) ? (ERole?)role : null).OfType<ERole>().ToList();
            return user.ToDomain(domainRoles);
        }
        public async Task<IList<string>> getUserRoles(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return null;
            var results = await userManager.GetRolesAsync(user);
            return results;
        }
        public async Task<User> GetUserById(Guid userId, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null) return null;
            var roles = await userManager.GetRolesAsync(user);
            var domainRoles = roles.Select(r =>
                Enum.TryParse<ERole>(r, out var role) ? (ERole?)role : null).OfType<ERole>().ToList();
            return user.ToDomain(domainRoles);
        }
        public async Task<bool> IsAnyUserWhoHaveEmailAndId(string email, Guid id)
        {
            return await appDbContext.Users.AnyAsync(u => u.Email == email && u.Id != id);
        }
        public async Task<IdentityResult?> DeleteUser(User user)
        {
            var userModel = user.ToModel();
            var result = await userManager.DeleteAsync(userModel);
            return result;
        }
        public async Task<bool> IsAnyUserWithUsernameAndEmailLikeThat(string username, string email)
        {
            return await appDbContext.Users.AnyAsync(u => u.UserName == username || u.Email == email);
        }
        public async Task<User> CreateUserWithDefaultRole(User user, CancellationToken cancellationToken)
        {
            var identityUser = user.ToModel();
            var result = await userManager.CreateAsync(identityUser, user.Password.HashValue);
            if (!result.Succeeded) throw new Exception("User creation failed: "+result.Errors.Select(e=>e.Description));
            await userManager.AddToRoleAsync(identityUser, ERole.User.ToString());
            return await GetUserById(identityUser.Id, cancellationToken);
        }
        public async Task<Dictionary<Guid, User>> GetByIds(List<Guid> userIds, CancellationToken cancellationToken)
        {
            var identityUsers = await userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync(cancellationToken);
            var userDictionary = new Dictionary<Guid, User>();
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
        }

        public async Task<string> GetUsernameById(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null) return null;
            return user.UserName!;
        }
    }
}
