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

        public Task<Dictionary<int, Media>> GetByIds(List<int> mediaIds, CancellationToken cancellationToken)
        {
            var medias = appDbContext.Medias
                .Where(m => mediaIds.Contains(m.Id))
                .ToDictionaryAsync(m => m.Id, m => m, cancellationToken);
            return medias;
        }

        public async Task<Media?> GetMediaById(int mediaId, CancellationToken cancellationToken)
        {
            var media = await appDbContext.Medias
                .Where(m => m.Id == mediaId)
                .FirstOrDefaultAsync(cancellationToken);
            return media;
        }
    }
}
