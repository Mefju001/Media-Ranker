using Application.Features.Common.Interfaces;
using Application.Features.Common.Models;
using Application.Features.Directors.Common;
using Application.Features.Genres.GetGenres;
using Application.Features.UserInteractions.Reviews.Common;

namespace Application.Features.Medias.Movies.Common
{
    public record MovieResponse(
        Guid id,
        string Title,
        string Description,
        GenreResponse GenreResponse,
        DirectorResponse DirectorResponse,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews,
        MediaStatsResponse MediaStats,
        TimeSpan Duration,
        string DistributionType,
        string status
        ):MediaResponse(id, Title, Description, GenreResponse, ReleaseDate, Language, MediaStats),IResponse;

}
