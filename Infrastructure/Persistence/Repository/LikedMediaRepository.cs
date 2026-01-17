using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class LikedMediaRepository : ILikedMediaRepository
    {
        public readonly AppDbContext appDbContext;
        public LikedMediaRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<bool> Any(int userId, int mediaId)
        {
            return await appDbContext.LikedMedias.AnyAsync(lm => lm.userId == userId && lm.mediaId == mediaId);
        }
        public async Task AddAsync(LikedMediaDomain likedMedia)
        {
            await appDbContext.LikedMedias.AddAsync(likedMedia);
        }
        public async Task<LikedMediaDomain?> GetByUserAndMediaId(int userId, int mediaId)
        {
            return await appDbContext.LikedMedias
                .Where(lm => lm.userId == userId && lm.mediaId == mediaId)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteByLikedMedia(int userId, int mediaId)
        {
            var result = await appDbContext.LikedMedias.FirstOrDefaultAsync(lm => lm.userId == userId && lm.mediaId == mediaId);
            if (result is null)
                return false;
            appDbContext.LikedMedias.Remove(result);
            return true;
        }
        public async Task<List<LikedMediaDomain>> GetAll()
        {
            return await appDbContext.LikedMedias.ToListAsync();
        }
        public async Task<List<LikedMediaDomain>> GetAllByUserId(int userId)
        {
            return await appDbContext.LikedMedias
                .Where(lm => lm.userId == userId)
                .ToListAsync();
        }
        public async Task<LikedMediaDomain?> GetById(int likedMediaId)
        {
            return await appDbContext.LikedMedias
                .Where(lm => lm.id == likedMediaId)
                .FirstOrDefaultAsync();
        }
        public async Task<List<LikedMediaDomain>> GetLikedForUser(int userId)
        {
            return await appDbContext.LikedMedias.Where(lm => lm.userId == userId).ToListAsync();
        }
    }
}
