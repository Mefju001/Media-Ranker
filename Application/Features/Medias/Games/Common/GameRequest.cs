using Application.Features.Genres.Common;
using Domain.Enums;

namespace Application.Features.Medias.Games.Common
{
    public record GameRequest
        (
        string Title,
        string Description,
        GenreRequest Genre,
        DateTime? ReleaseDate,
        string Language,
        EGameStatus GameStatus,
        string Developer,
        string? Engine,
        int PegiRating,
        List<EPlatform> Platforms,
        bool SupportsCrossPlay
        );
}
