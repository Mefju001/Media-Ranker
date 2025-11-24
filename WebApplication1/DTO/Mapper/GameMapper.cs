using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;

namespace WebApplication1.DTO.Mapper
{
    public static class GameMapper
    {
        public static GameResponse ToGameResponse(Game game)
        {
            return new GameResponse(
                game.Id,
                game.title,
                game.description,
                GenreMapper.ToResponse(game.genre),
                game.ReleaseDate,
                game.Language,
                game.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                game.Developer,
                game.Platform
                );
        }
        public static GameAvgResponse toGameAvgResponse(Game game, double average)
        {
            var gameResponse = ToGameResponse(game);
            return new GameAvgResponse(
                gameResponse,
                average);
        }
        public static void UpdateEntity(Game game, GameRequest gameRequest, Genre genre)
        {
            game.title = gameRequest.Title;
            game.description = gameRequest.Description;
            game.genre = genre;
            game.Language = gameRequest.Language;
            game.Developer = gameRequest.Developer;
            game.Platform = gameRequest.Platform;
            game.ReleaseDate = gameRequest.ReleaseDate!.Value;
        }
    }
}
