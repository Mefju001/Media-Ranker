using Application.Common.DTO.Response;
using Domain.Aggregate;

namespace Application.Mapper
{
    public static class GameMapper
    {
        public static GameResponse ToGameResponse(Game game, Genre genreDomain)
        {
            return new GameResponse(
                game.Id,
                game.Title,
                game.Description,
                GenreMapper.ToResponse(genreDomain),
                game.ReleaseDate.Value,
                game.Language,
                game.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                MediaStatsMapper.ToResponse(game.Stats!) ?? new MediaStatsResponse(0, 0, null),
                game.Developer,
                game.Platforms.ToList()
                );
        }

    }
}
