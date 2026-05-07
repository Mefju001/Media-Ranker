using Application.Features.Common.DTO.Response;
using Application.Features.Common.Interfaces;
using Application.Features.Genre.GetAll;
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
        ) : MediaResponse(id, Title, Description, Genre, ReleaseDate, Language, Reviews), IResponse;
}
