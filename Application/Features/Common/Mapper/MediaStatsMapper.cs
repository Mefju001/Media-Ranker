using Application.Features.Common.DTO.Response;
using Domain.Value_Object;

namespace Application.Features.Common.Mapper
{
    public static class MediaStatsMapper
    {
        public static MediaStatsResponse ToResponse(MediaStats mediaStats)
        {
            return new MediaStatsResponse(mediaStats.AverageRating, mediaStats.ReviewCount, mediaStats.LastCalculated);
        }
    }
}
