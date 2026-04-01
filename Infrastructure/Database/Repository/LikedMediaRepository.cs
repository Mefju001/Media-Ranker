using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repository
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
            return await appDbContext.LikedMedias.AnyAsync(lm => lm.UserId.Equals(userId) && lm.MediaId == mediaId, cancellationToken);
        }
        public async Task AddAsync(LikedMedia likedMedia, CancellationToken cancellationToken)
        {
            await appDbContext.LikedMedias.AddAsync(likedMedia, cancellationToken);
        }
        public async Task<LikedMedia?> GetByUserAndMediaId(Guid userId, int mediaId, CancellationToken cancellationToken)
        {
            return await appDbContext.LikedMedias
                .AsNoTrackingWithIdentityResolution()
                .Where(lm => lm.UserId == userId && lm.MediaId == mediaId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<bool> DeleteByLikedMedia(Guid userId, int mediaId, CancellationToken cancellationToken)
        {
            var results = await appDbContext.LikedMedias.Where(lm => lm.UserId == userId && lm.MediaId == mediaId).ExecuteDeleteAsync(cancellationToken);
            return results > 0;
        }
        public async Task<List<LikedMedia>> GetAll(CancellationToken cancellationToken)
        {
            return await appDbContext.LikedMedias.AsNoTrackingWithIdentityResolution().ToListAsync(cancellationToken);
        }
        public async Task<LikedMedia?> GetById(int likedMediaId, CancellationToken cancellationToken)
        {
            return await appDbContext.LikedMedias
                .AsNoTracking()
                .Where(lm => lm.Id == likedMediaId)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<List<LikedMedia>> GetLikedForUser(Guid userId, CancellationToken cancellationToken)
        {
            return await appDbContext.LikedMedias.AsNoTrackingWithIdentityResolution().Where(lm => lm.UserId == userId).ToListAsync(cancellationToken);
        }
    }
}
