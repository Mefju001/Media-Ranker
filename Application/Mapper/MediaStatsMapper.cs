using Application.Common.DTO.Response;
using Domain.Entity;
using Domain.Value_Object;

namespace Application.Mapper
{
    public static class MediaStatsMapper
    {
        public static MediaStatsResponse ToResponse(MediaStats mediaStats)
        {
            return new MediaStatsResponse(mediaStats.AverageRating, mediaStats.ReviewCount, mediaStats.LastCalculated);
        }
    }
}
