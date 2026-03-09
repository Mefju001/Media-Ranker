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
        public async Task<bool> Any(Guid userId, int mediaId, CancellationToken cancellationToken)
        {
            return await appDbContext.LikedMedias.AnyAsync(lm => lm.userId.Equals(userId) && lm.mediaId == mediaId, cancellationToken);
        }
        public async Task AddAsync(LikedMedia likedMedia, CancellationToken cancellationToken)
        {
            await appDbContext.LikedMedias.AddAsync(likedMedia, cancellationToken);
        }
        public async Task<LikedMedia?> GetByUserAndMediaId(Guid userId, int mediaId, CancellationToken cancellationToken)
        {
            return await appDbContext.LikedMedias
                .Where(lm => lm.userId.Equals(userId) && lm.mediaId == mediaId, cancellationToken)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> DeleteByLikedMedia(Guid userId, int mediaId, CancellationToken cancellationToken)
        {
            var result = await appDbContext.LikedMedias.FirstOrDefaultAsync(lm => lm.userId.Equals(userId) && lm.mediaId == mediaId, cancellationToken);
            if (result is null)
                return false;
            appDbContext.LikedMedias.Remove(result);
            return true;
        }
        public async Task<List<LikedMedia>> GetAll(CancellationToken cancellationToken)
        {
            return await appDbContext.LikedMedias.ToListAsync(cancellationToken);
        }
        public async Task<List<LikedMedia>> GetAllByUserId(Guid userId, CancellationToken cancellationToken)
        {
            return await appDbContext.LikedMedias
                .Where(lm => lm.userId.Equals(userId))
                .ToListAsync(cancellationToken);
        }
        public async Task<LikedMedia?> GetById(int likedMediaId, CancellationToken cancellationToken)
        {
            return await appDbContext.LikedMedias
                .Where(lm => lm.id == likedMediaId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<List<LikedMedia>> GetLikedForUser(Guid userId, CancellationToken cancellationToken)
        {
            return await appDbContext.LikedMedias.Where(lm => lm.userId.Equals(userId)).ToListAsync(cancellationToken);
        }
    }
}
