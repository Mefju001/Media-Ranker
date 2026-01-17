using Application.Common.DTO.Response;
using Application.Features.GamesServices.GameUpsert;
using Domain.Entity;

namespace Application.Mapper
{
    public static class GameMapper
    {
        public static GameResponse ToGameResponse(GameDomain game)
        {
            return new GameResponse(
                game.Id,
                game.Title,
                game.Description,
                new GenreResponse(0,""),// GenreMapper.ToResponse(game.genre),
                game.ReleaseDate,
                game.Language,
                game.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                MediaStatsMapper.ToResponse(game.Stats!) ?? new MediaStatsResponse(0, 0, 0, null),
                game.Developer,
                game.Platform
                );
        }

    }
}
