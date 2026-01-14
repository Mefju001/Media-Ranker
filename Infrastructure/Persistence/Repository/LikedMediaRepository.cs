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
    public class LikedMediaRepository: ILikedMediaRepository
    {
        public readonly AppDbContext appDbContext;
        public LikedMediaRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<bool> Any(int userId, int mediaId)
        {
            return await appDbContext.LikedMedias.AnyAsync(lm => lm.UserId == userId && lm.MediaId == mediaId);
        }
        public async Task AddAsync(LikedMedia likedMedia)
        {
            await appDbContext.LikedMedias.AddAsync(likedMedia);
        }
        public async Task<LikedMedia?> GetByUserAndMediaId(int userId, int mediaId)
        {
            return await appDbContext.LikedMedias
                .Where(lm => lm.UserId == userId && lm.MediaId == mediaId)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteByLikedMedia(int userId, int mediaId)
        {
            var result = await appDbContext.LikedMedias.FirstOrDefaultAsync(lm => lm.UserId == userId&&lm.MediaId == mediaId);
            if (result is null)
                return false;
            appDbContext.LikedMedias.Remove(result);
            return true;
        }
        public async Task<List<LikedMedia>> GetAll()
        {
            return await appDbContext.LikedMedias.ToListAsync();
        }
        public async Task<List<LikedMedia>> GetAllByUserId(int userId)
        {
            return await appDbContext.LikedMedias
                .Where(lm => lm.UserId == userId)
                .ToListAsync();
        }
        public async Task<LikedMedia?> GetById(int likedMediaId)
        {
            return await appDbContext.LikedMedias
                .Where(lm => lm.Id == likedMediaId)
                .FirstOrDefaultAsync();
        }
        public async Task<List<LikedMedia>> GetLikedForUser(int userId)
        {
            return await appDbContext.LikedMedias.Where(lm => lm.UserId == userId).ToListAsync();
        }
    }
}
