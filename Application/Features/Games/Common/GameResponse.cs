using Application.Features.Common.Interfaces;
using Application.Features.Genres.GetAll;
using Application.Features.Liked.Common;
using Application.Features.Movies.Common;
using Application.Features.Reviews.Common;

namespace Application.Features.Games.Command
{
    public record GameResponse(
        Guid id,
        string Title,
        string Description,
        GenreResponse Genre,
        DateTime ReleaseDate,
        string Language,
        List<ReviewResponse>? Reviews,
        MediaStatsResponse MediaStats,
        string Developer,
        string? Engine,
        int PegiRating,
        bool SupportsCrossPlay,
        List<string> Platforms
        ) : MediaResponse(id, Title, Description, Genre, ReleaseDate, Language, MediaStats), IResponse;
}
