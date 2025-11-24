using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Services
{
    public class StatsUpdateService : IStatsUpdateService
    {
        private readonly IUnitOfWork _unitOfWork;
        public StatsUpdateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task update(int mediaId, CancellationToken cancellationToken)
        {
            var media = await _unitOfWork.Medias.AsQueryable()
                .Include(x => x.Reviews)
                .Include(x => x.Stats)
                .SingleOrDefaultAsync(m => m.Id == mediaId, cancellationToken);
            if (media == null) return;
            if (media.Stats == null)
            {
                media.Stats = new MediaStats() { Media = media };
                await _unitOfWork.MediaStats.AddAsync(media.Stats);
            }
            int reviewCount = media.Reviews.Count();
            double? averageRating = reviewCount > 0 ? media.Reviews.Average(x => (double?)x.Rating) : null;

            media.Stats.ReviewCount = reviewCount;
            media.Stats.AverageRating = averageRating;
            media.Stats.LastCalculated = DateTime.UtcNow;
            await _unitOfWork.CompleteAsync();
        }
    }
}
