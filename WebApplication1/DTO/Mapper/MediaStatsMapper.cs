using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public static class MediaStatsMapper
    {
        public static MediaStatsResponse ToResponse(MediaStats mediaStats)
        {
            if (mediaStats == null)
            {
                return null;
            }
            return new MediaStatsResponse(mediaStats.AverageRating, mediaStats.ReviewCount, mediaStats.LastCalculated);
        }
    }
}
