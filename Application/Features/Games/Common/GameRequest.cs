using Application.Features.Genres.Common;

namespace Application.Features.Games.Command
{
    public record GameRequest
        (
        string Title,
        string Description,
        GenreRequest Genre,
        DateTime? ReleaseDate,
        string Language,
        string GameStatus,
        string Developer,
        string? Engine,
        int PegiRating,
        List<string> Platforms,
        bool SupportsCrossPlay
        );
}
