using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Application.Mapper
{
    public static class MediaStatsMapper
    {
        public static MediaStatsResponse ToResponse(MediaStats mediaStats)
        {
            return new MediaStatsResponse(mediaStats.MediaId, mediaStats.AverageRating, mediaStats.ReviewCount, mediaStats.LastCalculated);
        }
    }
}
