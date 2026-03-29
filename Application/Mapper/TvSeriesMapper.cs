using Application.Common.DTO.Response;
using Domain.Aggregate;
using Domain.Entity;

namespace Application.Mapper
{
    public class TvSeriesMapper
    {
        public static TvSeriesResponse ToTvSeriesResponse(TvSeries tvSeries, Genre genreDomain)
        {
            return new TvSeriesResponse(
                tvSeries.Id,
                tvSeries.Title,
                tvSeries.Description,
                GenreMapper.ToResponse(genreDomain),
                tvSeries.ReleaseDate.Value,
                tvSeries.Language,
                tvSeries.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                MediaStatsMapper.ToResponse(tvSeries.Stats!) ?? new MediaStatsResponse(0, 0, null),
                tvSeries.Seasons,
                tvSeries.Episodes,
                tvSeries.Network,
                tvSeries.Status);
        }
    }
}
