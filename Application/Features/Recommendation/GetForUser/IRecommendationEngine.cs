namespace Application.Features.Recommendation.GetForUser
{
    public interface IRecommendationEngine
    {
        Task<List<Media>> GetMediasAsync(UserProfileDto profile, EMediaRecommendationType eMediaRecommendationType, UserPreferencesDto prefs, CancellationToken cancellation);
    }
}
