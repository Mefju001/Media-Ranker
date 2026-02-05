using Application.Common.Interfaces;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class MediaRepository : IMediaRepository
    {
        private readonly AppDbContext appDbContext;

        public MediaRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public Task<Dictionary<int, MediaDomain>> GetByIds(List<int> mediaIds)
        {
            var medias = appDbContext.Medias
                .Where(m => mediaIds.Contains(m.Id))
                .ToDictionaryAsync(m => m.Id, m => m);
            return medias;
        }

        public async Task<MediaDomain> GetMediaById(int mediaId)
        {
            var media = await appDbContext.Medias
                .Where(m => m.Id == mediaId)
                .FirstOrDefaultAsync();
            if (media == null) throw new Exception("Media not found");
            return media;
        }
    }
}
