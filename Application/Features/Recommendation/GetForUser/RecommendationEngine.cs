using Application.Features.Common.Interfaces;
using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Recommendation.GetForUser
{
    internal class RecommendationEngine: IRecommendationEngine
    {
        private readonly IAppDbContext appDbContext;
        public RecommendationEngine(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<Media>> GetMediasAsync(UserProfileDto profile, UserPreferencesDto prefs, CancellationToken cancellation)
        {
            var movies = await GetMoviesAsync(profile, prefs, cancellation);
            var tvShows = await GetTvShowsAsync(profile, prefs, cancellation);
            var games = await GetGamesAsync(profile, prefs, cancellation);

            return movies.Cast<Media>()
                .Concat(tvShows)
                .Concat(games)
                .ToList();
        }
        private async Task<List<Movie>> GetMoviesAsync(UserProfileDto profile, UserPreferencesDto prefs, CancellationToken cancellation)
        {
            return await appDbContext.Medias
                .AsNoTracking()
                .AsSplitQuery()
                .OfType<Movie>()
                .Where(m => !profile.FavLikedMediaIds.Contains(m.Id))
                .Where(m => !profile.DislikedMediaIds.Contains(m.Id) && !profile.IgnoredMediaIds.Contains(m.Id))
                .Where(m => prefs.GenreIds.Contains(m.GenreId) || prefs.DirectorIds.Contains(m.DirectorId))
                .Select(m => new
                {
                    Movie = m,
                    Score = (prefs.GenreIds.Contains(m.GenreId) ? 3 : 0) +
                            (prefs.DirectorIds.Contains(m.DirectorId) ? 5 : 0)
                })
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.Movie.Stats.AverageRating)
                .Take(3)
                .Select(x => x.Movie)
                .ToListAsync(cancellation);
        }

        private async Task<List<Domain.Aggregate.TvSeries>> GetTvShowsAsync(UserProfileDto profile, UserPreferencesDto prefs, CancellationToken cancellation)
        {
            return await appDbContext.Medias
                .AsNoTracking()
                .AsSplitQuery()
                .OfType<Domain.Aggregate.TvSeries>()
                .Where(s => !profile.FavLikedMediaIds.Contains(s.Id))
                .Where(s => !profile.DislikedMediaIds.Contains(s.Id) && !profile.IgnoredMediaIds.Contains(s.Id))
                .Where(s => prefs.GenreIds.Contains(s.GenreId) || prefs.TvShowsPlatforms.Contains(s.Network))
                .Select(s => new
                {
                    TvShow = s,
                    Score = (prefs.GenreIds.Contains(s.GenreId) ? 3 : 0) +
                            (prefs.TvShowsPlatforms.Contains(s.Network) ? 4 : 0)
                })
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.TvShow.Stats.AverageRating)
                .Take(3)
                .Select(x => x.TvShow)
                .ToListAsync(cancellation);
        }

        private async Task<List<Game>> GetGamesAsync(UserProfileDto profile, UserPreferencesDto prefs, CancellationToken cancellation)
        {
            return await appDbContext.Medias
                .AsNoTracking()
                .AsSplitQuery()
                .OfType<Game>()
                .Where(g => !profile.FavLikedMediaIds.Contains(g.Id))
                .Where(g => !profile.DislikedMediaIds.Contains(g.Id) && !profile.IgnoredMediaIds.Contains(g.Id))
                .Where(g => prefs.GenreIds.Contains(g.GenreId) || prefs.Developers.Contains(g.Developer))
                .Select(g => new
                {
                    Game = g,
                    Score = (prefs.GenreIds.Contains(g.GenreId) ? 3 : 0) +
                            (prefs.Developers.Contains(g.Developer) ? 5 : 0) +
                            (prefs.GamesPlatforms.Any(p => g.Platforms.Contains(p)) ? 4 : 0)
                })
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.Game.Stats.AverageRating)
                .Take(3)
                .Select(x => x.Game)
                .ToListAsync(cancellation);
        }
    }
}
