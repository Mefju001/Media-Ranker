using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Features.Games.MovieUpsert;
using WebApplication1.Domain.Entities;

namespace WebApplication1.Application.Mapper
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
                MediaStatsMapper.ToResponse(game.Stats!) ?? new MediaStatsResponse(0, 0, 0, null),
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
        public static void UpdateEntity(Game game, UpsertGameCommand gameRequest, Genre genre)
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
