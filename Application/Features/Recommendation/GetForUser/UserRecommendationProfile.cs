using Domain.Enums;

namespace Application.Features.Recommendation.GetForUser
{
    public record UserProfileDto(
        List<Guid> FavLikedMediaIds,
        List<Guid> WantToWatchMediaIds,
        List<Guid> DislikedMediaIds,
        List<Guid> IgnoredMediaIds
    );

    public record UserPreferencesDto(
        HashSet<Guid> GenreIds,
        HashSet<Guid> DirectorIds,
        HashSet<string> Developers,
        HashSet<string> TvShowsPlatforms,
        HashSet<string> GamesPlatforms
    );

    public static class UserRecommendationProfileDto
    {
        public static UserProfileDto Create(HashSet<Domain.Entity.UserInteractions> interactions)
        {
            return new UserProfileDto(
                FavLikedMediaIds: interactions.Where(ui => ui.RatingVote == ERatingVote.Liked).Select(ui => ui.MediaId).ToList(),
                WantToWatchMediaIds: interactions.Where(ui => ui.TypeInteractions == ETypeInteractions.Planned).Select(ui => ui.MediaId).ToList(),
                DislikedMediaIds: interactions.Where(ui => ui.RatingVote == ERatingVote.Disliked).Select(ui => ui.MediaId).ToList(),
                IgnoredMediaIds: interactions.Where(ui => ui.TypeInteractions == ETypeInteractions.Ignored).Select(ui => ui.MediaId).ToList()
            );
        }
    }
}
