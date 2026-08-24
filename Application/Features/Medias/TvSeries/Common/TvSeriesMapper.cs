using Application.Features.Genres.GetAll;
using Application.Features.Genres.GetGenres;
using Application.Features.Medias.Movies.Common;
using Application.Features.UserInteractions.Reviews.Common;
using domain = Domain.Aggregate;

namespace Application.Features.Medias.TvSeries.Common
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
                tvSeries.SeasonAndEpisode.Seasons,
                tvSeries.SeasonAndEpisode.Episodes,
                tvSeries.Network,
                tvSeries.Status.ToString());
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
                tvSeries.SeasonAndEpisode.Seasons,
                tvSeries.SeasonAndEpisode.Episodes,
                tvSeries.Network,
                tvSeries.Status.ToString());
        }
    }
}
