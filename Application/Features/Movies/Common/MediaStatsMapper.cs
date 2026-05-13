using Domain.Value_Object;

namespace Application.Features.Movies.Common
{
    public static class MediaStatsMapper
    {
        public static MediaStatsResponse ToResponse(MediaStats mediaStats)
        {
            return new MediaStatsResponse(mediaStats.AverageRating, mediaStats.ReviewCount, mediaStats.LastCalculated);
        }
    }
}
