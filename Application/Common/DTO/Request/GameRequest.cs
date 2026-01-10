using WebApplication1.Domain.ValueObjects;

namespace WebApplication1.Application.Common.DTO.Request
{
    public record GameRequest
        (
        string Title,
        string Description,
        GenreRequest Genre,
        DateTime? ReleaseDate,
        string Language,
        string? Developer,
        EPlatform Platform
        );
}
