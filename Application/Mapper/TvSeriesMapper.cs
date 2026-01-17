using Application.Common.DTO.Response;
using Application.Features.TvSeriesServices.TvSeriesUpsert;
using Domain.Entity;

namespace Application.Mapper
{
    public class TvSeriesMapper
    {
        public static TvSeriesResponse ToTvSeriesResponse(TvSeriesDomain tvSeries)
        {
            return new TvSeriesResponse(
                tvSeries.Id,
                tvSeries.Title,
                tvSeries.Description,
                new GenreResponse(0, ""),//GenreMapper.ToResponse(tvSeries.g),
                tvSeries.ReleaseDate,
                tvSeries.Language,
                tvSeries.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                new MediaStatsResponse(0, 0.0, 0, DateTime.UtcNow),//MediaStatsMapper.ToResponse(tvSeries.Stats!) ?? new MediaStatsResponse(0, 0, 0, null),
                tvSeries.Seasons,
                tvSeries.Episodes,
                tvSeries.Network,
                tvSeries.Status);
        }
    }
}
