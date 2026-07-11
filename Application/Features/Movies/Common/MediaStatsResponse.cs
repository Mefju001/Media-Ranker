namespace Application.Features.Movies.Common
{
    // maybe add mediastats for game and tv show as well? or maybe make it more generic and then create specific ones for movies, games and tv shows?
    public record MediaStatsResponse(double? AverageRating, int? ReviewCount, DateTime? LastCalculated);
}
