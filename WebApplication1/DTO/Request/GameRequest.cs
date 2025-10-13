using WebApplication1.Models;

namespace WebApplication1.DTO.Request
{
    public record GameRequest
        (
        string Title,
        string Description,
        GenreRequest Genre,
        DateTime ReleaseDate,
        string Language,
        string? Developer,
        EPlatform Platform
        );
}
