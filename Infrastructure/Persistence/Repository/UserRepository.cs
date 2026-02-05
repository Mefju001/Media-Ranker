using Application.Common.Interfaces;
using Domain.Entity;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext appDbContext;
        public UserRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<UserDomain> GetUserByUsername(string username)
        {
            var user = await appDbContext.Users
                .Where(u => u.username == username)
                .FirstOrDefaultAsync();
            if (user == null) throw new NotFoundException("User not found");
            return user;
        }
        public async Task<UserDomain> GetUserIdByUsername(string username)
        {
            var user = await appDbContext.Users
                .Where(u => u.username == username)
                .FirstOrDefaultAsync();
            if (user == null) throw new NotFoundException("User not found");
            return user;
        }
        public async Task<UserDomain> GetUserById(int userId)
        {
            var user = await appDbContext.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
            if (user == null) throw new NotFoundException("User not found");
            return user;
        }
        public async Task<bool> IsAnyUserWhoHaveEmailAndId(string email, int id)
        {
            return await appDbContext.Users.AnyAsync(u => u.email == email && u.Id == id);
        }
        public async Task DeleteUser(int userId)
        {
            var user = await appDbContext.Users
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();
            if (user == null) throw new NotFoundException("User not found");
            appDbContext.Users.Remove(user);
        }
        public async Task<bool>IsAnyUserWithUsernameAndEmailLikeThat(string username, string email)
        {
            return await appDbContext.Users.AnyAsync(u=>u.username == username&&u.email==email);
        }
        public async Task<UserDomain> AddUser(UserDomain user)
        {
           var result =  await appDbContext.Users.AddAsync(user);
            return result.Entity;
        }

        public Task<Dictionary<int, UserDomain>> GetByIds(List<int> userIds)
        {
            var users = appDbContext.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u);
            return users;
        }
    }
}
