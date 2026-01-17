using Application.Common.DTO.Response;
using Domain.Entity;

namespace Application.Mapper
{
    public static class MediaStatsMapper
    {
        public static MediaStatsResponse ToResponse(MediaStatsDomain mediaStats)
        {
            return new MediaStatsResponse(mediaStats.MediaId, mediaStats.AverageRating, mediaStats.ReviewCount, mediaStats.LastCalculated);
        }
    }
}
