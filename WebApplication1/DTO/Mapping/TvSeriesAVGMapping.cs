using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public class TvSeriesAVGMapping
    {
        public static TvSeriesAVGResponse ToResponse(TvSeries tvSeries,double avg)
        {
            return new TvSeriesAVGResponse(
                tvSeries.title,
                tvSeries.description,
                GenreMapping.ToResponse(tvSeries.genre),
                tvSeries.ReleaseDate,
                tvSeries.Language,
                tvSeries.Reviews?.Select(r => ReviewMapping.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                tvSeries.Seasons,
                tvSeries.Episodes,
                tvSeries.Network,
                tvSeries.Status,
                avg
                );
        }
    }
}
