using Domain.Value_Object;

namespace Application.Features.Medias.Movies.Common
{
    public static class MediaStatsMapper
    {
        public static MediaStatsResponse ToResponse(MediaStats mediaStats)
        {
            return new MediaStatsResponse(mediaStats.AverageRating, mediaStats.ReviewCount, mediaStats.LastCalculated);
        }
    }
}
