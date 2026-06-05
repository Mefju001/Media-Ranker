using Domain.Entity;
using Domain.Enums;

namespace Application.Features.Recommendation.GetForUser
{
    public record UserProfileData(
        List<Guid> FavLikedMediaIds,
        List<Guid> WantToWatchMediaIds,
        List<Guid> DislikedMediaIds,
        List<Guid> IgnoredMediaIds
    );

    public record UserPreferencesData(
        List<Guid> GenreIds,
        List<Guid> DirectorIds,
        List<string> Developers,
        List<string> TvShowsPlatforms,
        List<EPlatform> GamesPlatforms
    );

    public static class UserRecommendationProfile
    {
        public static UserProfileData Create(List<UserInteractions> interactions)
        {
            return new UserProfileData(
                FavLikedMediaIds: interactions.Where(ui => ui.RatingVote == ERatingVote.Liked).Select(ui => ui.MediaId).ToList(),
                WantToWatchMediaIds: interactions.Where(ui => ui.TypeInteractions == ETypeInteractions.WANT_TO_WATCH).Select(ui => ui.MediaId).ToList(),
                DislikedMediaIds: interactions.Where(ui => ui.RatingVote == ERatingVote.Disliked).Select(ui => ui.MediaId).ToList(),
                IgnoredMediaIds: interactions.Where(ui => ui.TypeInteractions == ETypeInteractions.IGNORED).Select(ui => ui.MediaId).ToList()
            );
        }
    }
}
