using WebApplication1.Models;

namespace WebApplication1.DTO.Response
{
    public record GameAvgResponse(
        int id,
        string Title,
        string Description,
        GenreResponse Genre,
        DateTime ReleaseDate,
        string? Language,
        List<ReviewResponse>? Reviews,

        string? Developer,
        EPlatform Platform,
        double Average
        ) : MediaResponse(id, Title, Description, Genre, ReleaseDate, Language, Reviews)
    {
        public GameAvgResponse(GameResponse game, double avg) : this(game.id, game.Title,
            game.Description, game.Genre, game.ReleaseDate, game.Language, game.Reviews,
            game.Developer, game.Platform, avg)
        { }
    }
}
