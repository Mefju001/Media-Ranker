using Application.Features.Common.DTO.Request;
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
        List<EPlatform> Platforms
        );
}
