using Application.Common.Interfaces;
using Domain.Entity;
using Domain.Enums;
using Domain.Exceptions;
using Infrastructure.DBModels;
using Infrastructure.DBModels.Extensions;
using MediatR;
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
        
        public async Task<User> GetUserByUsername(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return null;
            var roles = await userManager.GetRolesAsync(user);
            var domainRoles = roles.Select(r =>
                Enum.TryParse<ERole>(r, out var role)? Role.Create(role):null).Where(r=>r!=null).ToList();
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
            Enum.TryParse<ERole>(r, out var role) ? Role.Create(role) : null).Where(r => r != null).ToList();
            return user.ToDomain(domainRoles);
        }
        public async Task<IList<string>> getUserRoles(string username)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return null;
            var results = await userManager.GetRolesAsync(user);
            return results;
        }
        public async Task<User> GetUserById(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null) return null;
            var roles = await userManager.GetRolesAsync(user);
            var domainRoles = roles.Select(r =>
                Enum.TryParse<ERole>(r, out var role) ? Role.Create(role) : null).Where(r => r != null).ToList();
            return user.ToDomain(domainRoles);
        }
        public async Task<bool> IsAnyUserWhoHaveEmailAndId(string email, Guid id)
        {
            return await appDbContext.Users.AnyAsync(u => u.Email == email && u.Id == id);
        }
        public async Task DeleteUser(Guid userId)
        {
            var user = await appDbContext.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
            if (user == null) throw new NotFoundException("User not found");
            appDbContext.Users.Remove(user);
        }
        public async Task<bool>IsAnyUserWithUsernameAndEmailLikeThat(string username, string email)
        {
            return await appDbContext.Users.AnyAsync(u=>u.UserName == username&&u.Email ==email);
        }
        public async Task CreateUserWithDefaultRole(User user)
        {
            var identityUser = user.ToModel();
            var result = await userManager.CreateAsync(identityUser, user.password.HashValue);

            if (!result.Succeeded) throw new Exception("User creation failed");

            await userManager.AddToRoleAsync(identityUser, "User");
        }
        public async Task<Dictionary<Guid, User>> GetByIds(List<Guid> userIds)
        {
            throw new NotImplementedException();
            /*var results = await userManager.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u=>u.ToDomain())
                .ToDictionaryAsync(u => u.Id, u => u);
            return results;*/
        }

        public async Task<string> GetUsernameById(Guid id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if(user == null)return null;
            return user.UserName!;
        }
    }
}
