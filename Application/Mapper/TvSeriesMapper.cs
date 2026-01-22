using Application.Common.DTO.Response;
using Application.Features.TvSeriesServices.TvSeriesUpsert;
using Domain.Entity;
using System.Linq.Expressions;

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
        public static Expression<Func<TvSeriesDomain,TvSeriesResponse>> ToDto = tv => new TvSeriesResponse
        (
            tv.Id,
            tv.Title,
            tv.Description,
            GenreMapper.ToDto(tv.GenreDomain),
            tv.ReleaseDate,
            tv.Language,
            tv.Reviews.Select(r => ReviewMapper.ToResponse(r)).ToList(),
            MediaStatsMapper.ToResponse(tvSeries.Stats!) ?? new MediaStatsResponse(0, 0, 0, null),
            tv.Seasons,
            tv.Episodes,
            tv.Network,
            tv.Status
        );
    }
}
