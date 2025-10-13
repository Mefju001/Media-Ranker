using System.IO;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapping
{
    public static class GameMapping
    {
        public static GameResponse ToResponse(Game game)
        {
            return new GameResponse(
                game.title,
                game.description,
                GenreMapping.ToResponse(game.genre),
                game.ReleaseDate,
                game.Language,
                game.Reviews?.Select(r => ReviewMapping.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                game.Developer,
                game.Platform
                );
        }
        public static void UpdateEntity(Game game ,GameRequest gameRequest, Genre genre)
        {
            game.title = gameRequest.Title;
            game.description = gameRequest.Description;
            game.genre = genre;
            game.Language = gameRequest.Language;
            game.Developer = gameRequest.Developer;
            game.Platform = gameRequest.Platform;
            game.ReleaseDate = gameRequest.ReleaseDate.Value;
        }
    }
}
