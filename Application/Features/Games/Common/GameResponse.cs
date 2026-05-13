using Application.Features.Common.Interfaces;
using Application.Features.Genres.GetAll;
using Application.Features.Liked.Common;
using Application.Features.Movies.Common;
using Application.Features.Reviews.Common;
using Domain.Enums;

namespace Application.Features.Games.Command
{
    public record GameResponse(
        Guid id,
        string Title,
        string Description,
        GenreResponse Genre,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews,
        MediaStatsResponse MediaStats,
        string? Developer,
        List<EPlatform> Platforms
        ) : MediaResponse(id, Title, Description, Genre, ReleaseDate, Language, MediaStats), IResponse;
}
