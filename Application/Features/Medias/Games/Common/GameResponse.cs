using Application.Features.Common.Interfaces;
using Application.Features.Common.Models;
using Application.Features.Genres.GetGenres;
using Application.Features.Medias.Movies.Common;
using Application.Features.UserInteractions.Reviews.Common;

namespace Application.Features.Medias.Games.Common
{
    public record GameResponse(
        Guid id,
        string Title,
        string Description,
        GenreResponse GenreResponse,
        DateTime ReleaseDate,
        string Language,
        List<ReviewResponse>? Reviews,
        MediaStatsResponse MediaStats,
        string Developer,
        string? Engine,
        int PegiRating,
        bool SupportsCrossPlay,
        List<string> Platforms
        ) : MediaResponse(id, Title, Description, GenreResponse, ReleaseDate, Language, MediaStats), IResponse;
}
