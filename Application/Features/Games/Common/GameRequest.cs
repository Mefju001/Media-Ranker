using Application.Features.Genres.Common;
using Domain.Enums;

namespace Application.Features.Games.Command
{
    public record GameRequest
        (
        string Title,
        string Description,
        GenreRequest Genre,
        DateTime? ReleaseDate,
        string Language,
        string? Developer,
        List<string> Platforms
        );
}
