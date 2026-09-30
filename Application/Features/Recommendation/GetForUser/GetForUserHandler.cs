using Application.Features.Common.Interfaces;
using Application.Features.Common.Mapping;
using Application.Features.Common.Models;
using Domain.Aggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Recommendation.GetForUser
{
    internal class GetForUserHandler : IRequestHandler<GetForUserQuery, List<MediaResponse>>
    {
        private readonly IAppDbContext appDbContext;
        private readonly IRecommendationEngine recommendationEngine;
        public GetForUserHandler(IAppDbContext appDbContext, IRecommendationEngine recommendationEngine)
        {
            this.appDbContext = appDbContext;
            this.recommendationEngine = recommendationEngine;
        }
        public async Task<List<MediaResponse>> Handle(GetForUserQuery request, CancellationToken cancellationToken)
        {
            var userInteractions = await appDbContext.UserInteractions
                        .AsNoTracking()
                        .Where(ui => ui.UserId == request.UserId)
                        .ToListAsync(cancellationToken);

            if (!userInteractions.Any())
            {
                return await ColdStart(request.RecommendationType, cancellationToken);
            }
            var userRecommendationProfile = UserRecommendationProfileDto.Create(userInteractions.ToHashSet());
            var userPreferencesData = await FetchPreferencesAsync(userRecommendationProfile, cancellationToken);
            var recommendationResults = await recommendationEngine.GetMediasAsync(userRecommendationProfile, request.RecommendationType, userPreferencesData, cancellationToken);
            if(!recommendationResults.Any())
            {
                return await ColdStart(request.RecommendationType, cancellationToken);
            }
            return await MapToResponsesAsync(recommendationResults, cancellationToken);
        }
        private async Task<List<MediaResponse>> MapToResponsesAsync(List<Media> allRecommended, CancellationToken cancellation)
        {
            var recommendedGenreIds = allRecommended.Select(m => m.GenreId).ToHashSet();
            var recommendedDirectorIds = allRecommended.OfType<Movie>().Select(m => m.DirectorId).ToHashSet();

            var genres = await appDbContext.Set<Genre>().AsNoTracking().Where(g => recommendedGenreIds.Contains(g.Id)).ToDictionaryAsync(g => g.Id, cancellation);
            var directors = await appDbContext.Set<Director>().AsNoTracking().Where(d => recommendedDirectorIds.Contains(d.Id)).ToDictionaryAsync(d => d.Id, cancellation);

            return allRecommended.Select(m =>
            {
                Director? director = null;
                genres.TryGetValue(m.GenreId, out var genre);
                if (m is Movie movie)
                {
                    directors.TryGetValue(movie.DirectorId, out director);
                }
                return m.ToResponse(genre, director);
            }).ToList();
        }
        private async Task<UserPreferencesDto> FetchPreferencesAsync(UserProfileDto profile, CancellationToken cancellation)
        {
            var favoriteMedias = await appDbContext.Medias
                        .AsNoTracking()
                        .Where(m => profile.FavLikedMediaIds.Contains(m.Id) || profile.WantToWatchMediaIds.Contains(m.Id))
                        .ToListAsync(cancellation);

            return new UserPreferencesDto(
                GenreIds: favoriteMedias.Select(m => m.GenreId).ToHashSet(),
                DirectorIds: favoriteMedias.OfType<Movie>().Select(m => m.DirectorId).ToHashSet(),
                Developers: favoriteMedias.OfType<Game>().Select(m => m.Details.Developer).ToHashSet(),
                TvShowsPlatforms: favoriteMedias.OfType<TvSeries>().Select(s => s.Network).ToHashSet(),
                GamesPlatforms: favoriteMedias.OfType<Game>().SelectMany(g => g.Platforms.Values).ToHashSet()
            );
        }
        private async Task<List<MediaResponse>> ColdStart(EMediaRecommendationType eMediaRecommendationType, CancellationToken cancellation)
        {
            var medias = await GetMediasAsync(eMediaRecommendationType, cancellation);
            var movieDirectorsIds = medias.OfType<Movie>().Select(m => m.DirectorId).ToHashSet();
            var directors = await appDbContext.Set<Director>().AsNoTracking().Where(d => movieDirectorsIds.Contains(d.Id)).ToDictionaryAsync(d => d.Id, cancellation);
            var genreIds = medias.Select(m => m.GenreId).ToHashSet();
            var genres = await appDbContext.Set<Genre>().AsNoTracking().Where(g => genreIds.Contains(g.Id)).ToDictionaryAsync(d => d.Id, cancellation);
            return medias.Select(m =>
            {
                Director? director = null;
                genres.TryGetValue(m.GenreId, out var genre);
                if (m is Movie movie)
                {
                    directors.TryGetValue(movie.DirectorId, out director);
                }
                return m.ToResponse(genre, director);
            }).ToList();
        }
        private async Task<List<Media>> GetMediasAsync(EMediaRecommendationType eMediaRecommendationType, CancellationToken cancellation)
        {
            return eMediaRecommendationType switch
            {
                EMediaRecommendationType.All => await GetAll(),
                EMediaRecommendationType.Movie => await GetBy<Movie>(size:9, cancellation),
                EMediaRecommendationType.TvShow => await GetBy<TvSeries>(size:9, cancellation),
                EMediaRecommendationType.Game => await GetBy<Game>(size:9, cancellation),
                _ => throw new ArgumentOutOfRangeException(nameof(eMediaRecommendationType), eMediaRecommendationType, null),
            };  
            
        }
        private async Task<List<Media>>GetAll()
        {
            var games = await appDbContext.Medias
                                .AsNoTracking()
                                .OfType<Game>()
                                .OrderByDescending(m => m.Stats.AverageRating)
                                .Take(3)
                                .ToListAsync();
            var movies = await appDbContext.Medias
                                .AsNoTracking()
                                .OfType<Movie>()
                                .OrderByDescending(m => m.Stats.AverageRating)
                                .Take(3)
                                .ToListAsync();
            var tvSeries = await appDbContext.Medias
                                .AsNoTracking()
                                .OfType<TvSeries>()
                                .OrderByDescending(m => m.Stats.AverageRating)
                                .Take(3)
                                .ToListAsync();
            return games.Cast<Media>().Concat(movies).Concat(tvSeries).ToList();
        }
        private async Task<List<Media>>GetBy<T>(int size,CancellationToken cancellation) where T : Media
        {
            return await appDbContext.Medias
                    .AsNoTracking()
                    .OfType<T>()
                    .OrderByDescending(m => m.Stats.AverageRating)
                    .Take(size)
                    .Cast<Media>()
                    .ToListAsync();
        }
    }
}
