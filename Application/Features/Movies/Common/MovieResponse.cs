using Application.Features.Common.Interfaces;
using Application.Features.Directors.Common;
using Application.Features.Genres.GetAll;
using Application.Features.Liked.Common;
using Application.Features.Reviews.Common;

namespace Application.Features.Movies.Common
{
    public record MovieResponse(
        Guid id,
        string Title,
        string Description,
        GenreResponse Genre,
        DirectorResponse Director,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews,
        MediaStatsResponse MediaStats,
        TimeSpan Duration,
        bool IsCinemaRelease

        ):MediaResponse(id, Title, Description, Genre, ReleaseDate, Language, MediaStats),IResponse;

}
