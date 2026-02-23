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
        public async Task<bool> Any(Guid userId, int mediaId)
        {
            return await appDbContext.LikedMedias.AnyAsync(lm => lm.userId.Equals(userId) && lm.mediaId == mediaId);
        }
        public async Task AddAsync(LikedMedia likedMedia)
        {
            await appDbContext.LikedMedias.AddAsync(likedMedia);
        }
        public async Task<LikedMedia?> GetByUserAndMediaId(Guid userId, int mediaId)
        {
            return await appDbContext.LikedMedias
                .Where(lm => lm.userId.Equals(userId) && lm.mediaId == mediaId)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteByLikedMedia(Guid userId, int mediaId)
        {
            var result = await appDbContext.LikedMedias.FirstOrDefaultAsync(lm => lm.userId.Equals(userId) && lm.mediaId == mediaId);
            if (result is null)
                return false;
            appDbContext.LikedMedias.Remove(result);
            return true;
        }
        public async Task<List<LikedMedia>> GetAll()
        {
            return await appDbContext.LikedMedias.ToListAsync();
        }
        public async Task<List<LikedMedia>> GetAllByUserId(Guid userId)
        {
            return await appDbContext.LikedMedias
                .Where(lm => lm.userId.Equals(userId))
                .ToListAsync();
        }
        public async Task<LikedMedia?> GetById(int likedMediaId)
        {
            return await appDbContext.LikedMedias
                .Where(lm => lm.id == likedMediaId)
                .FirstOrDefaultAsync();
        }
        public async Task<List<LikedMedia>> GetLikedForUser(Guid userId)
        {
            return await appDbContext.LikedMedias.Where(lm => lm.userId.Equals(userId)).ToListAsync();
        }
    }
}
