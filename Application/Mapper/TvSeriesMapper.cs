using Application.Common.DTO.Response;
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
                GenreMapper.ToResponse(tvSeries.GenreDomain),
                tvSeries.ReleaseDate,
                tvSeries.Language,
                tvSeries.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                MediaStatsMapper.ToResponse(tvSeries.Stats!) ?? new MediaStatsResponse(0, 0, 0, null),
                tvSeries.Seasons,
                tvSeries.Episodes,
                tvSeries.Network,
                tvSeries.Status);
        }
        public static Expression<Func<TvSeriesDomain, TvSeriesResponse>> ToDto = tv => new TvSeriesResponse
        (
            tv.Id,
            tv.Title,
            tv.Description,
            new GenreResponse
            (tv.GenreId, tv.GenreDomain.name),
            tv.ReleaseDate,
            tv.Language,
            tv.Reviews.Select(r => new ReviewResponse(r.Id, r.Media.Id, r.User.username, r.Rating, r.Comment, r.CreatedAt, r.LastModifiedAt)).ToList(),
            new MediaStatsResponse(tv.Stats.MediaId, tv.Stats.AverageRating, tv.Stats.ReviewCount, tv.Stats.LastCalculated),
            tv.Seasons,
            tv.Episodes,
            tv.Network,
            tv.Status
        );
    }
}
