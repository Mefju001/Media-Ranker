using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Exceptions;
using WebApplication1.Infrastructure.Persistence;

namespace Infrastructure.Persistence.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext appDbContext;
        public UserRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<User> GetUserIdByUsername(string username)
        {
            var user = await appDbContext.Users
                .Where(u => u.username == username)
                .FirstOrDefaultAsync();
            if (user == null) throw new NotFoundException("User not found");
            return user;
        }
        public async Task<User> GetUserById(int userId)
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
    }
}
