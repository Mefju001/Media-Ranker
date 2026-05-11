using Application.Features.Genres.GetAll;
using Application.Features.Movies.Common;
using Application.Features.Reviews.Common;
using domain = Domain.Aggregate;

namespace Application.Features.TvSeries.Common
{
    public class TvSeriesMapper
    {
        public static TvSeriesResponse ToTvSeriesResponse(domain.TvSeries tvSeries, domain.Genre genreDomain)
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
        public static TvSeriesResponse ToTvSeriesResponse(domain.TvSeries tvSeries, GenreResponse genreResponse)
        {
            return new TvSeriesResponse(
                tvSeries.Id,
                tvSeries.Title,
                tvSeries.Description,
                genreResponse,
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
