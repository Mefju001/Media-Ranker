using Application.Features.Common.Interfaces;
using Application.Features.Liked.Common;
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
                return await ColdStart(cancellationToken);
            }
            var userRecommendationProfile = UserRecommendationProfileDto.Create(userInteractions);
            var userPreferencesData = await FetchPreferencesAsync(userRecommendationProfile, cancellationToken);
            var recommendationResults = await recommendationEngine.GetMediasAsync(userRecommendationProfile, userPreferencesData, cancellationToken);
            return await MapToResponsesAsync(recommendationResults, cancellationToken);
        }
        private async Task<List<MediaResponse>> MapToResponsesAsync(List<Media> allRecommended, CancellationToken cancellation)
        {
            var recommendedGenreIds = allRecommended.Select(m => m.GenreId).Distinct().ToList();
            var recommendedDirectorIds = allRecommended.OfType<Movie>().Select(m => m.DirectorId).Distinct().ToList();

            var genres = await appDbContext.Set<Genre>().AsNoTracking().Where(g => recommendedGenreIds.Contains(g.Id)).ToDictionaryAsync(g => g.Id, cancellation);
            var directors = await appDbContext.Set<Director>().AsNoTracking().Where(d => recommendedDirectorIds.Contains(d.Id)).ToDictionaryAsync(d => d.Id, cancellation);

            return allRecommended.Select(m =>
            {
                Director? director = null;
                if (m is Movie movie)
                {
                    directors.TryGetValue(movie.DirectorId, out director);
                }
                return m.ToResponse(genres[m.GenreId], director);
            }).ToList();
        }
        private async Task<UserPreferencesDto> FetchPreferencesAsync(UserProfileDto profile, CancellationToken cancellation)
        {
            var favoriteMedias = await appDbContext.Medias
                        .AsNoTracking()
                        .Where(m => profile.FavLikedMediaIds.Contains(m.Id) || profile.WantToWatchMediaIds.Contains(m.Id))
                        .ToListAsync(cancellation);

            return new UserPreferencesDto(
                GenreIds: favoriteMedias.Select(m => m.GenreId).Distinct().ToList(),
                DirectorIds: favoriteMedias.OfType<Movie>().Select(m => m.DirectorId).Distinct().ToList(),
                Developers: favoriteMedias.OfType<Game>().Select(m => m.Developer).Distinct().ToList(),
                TvShowsPlatforms: favoriteMedias.OfType<Domain.Aggregate.TvSeries>().Select(s => s.Network).Distinct().ToList(),
                GamesPlatforms: favoriteMedias.OfType<Game>().SelectMany(g => g.Platforms).Distinct().ToList()
            );
        }
        private async Task<List<MediaResponse>> ColdStart(CancellationToken cancellation)
        {
            var medias = await appDbContext.Medias
                                            .AsSplitQuery()
                                            .AsNoTracking()
                                            .Where(m => m.ReleaseDate.Value > DateTime.UtcNow.AddMonths(-6))
                                            .OrderByDescending(m => m.Stats.AverageRating)
                                            .Join(appDbContext.Set<Genre>(),
                                            t => t.GenreId, g => g.Id,
                                            (media, genre) => new { media, genre })
                                            .Take(10)
                                            .ToListAsync(cancellation);
            var movieDirectorsIds = medias.Select(x => x.media).OfType<Movie>().Select(m => m.DirectorId).Distinct().ToList();
            var directors = await appDbContext.Set<Director>().AsNoTracking().Where(d => movieDirectorsIds.Contains(d.Id)).ToDictionaryAsync(d => d.Id, cancellation);
            var response = medias.Select(m =>
            {
                Director? director = null;
                if (m.media is Movie movie)
                {
                    directors.TryGetValue(movie.DirectorId, out director);
                }
                return m.media.ToResponse(m.genre, director);
            }).ToList();
            return response;
        }
    }
}
