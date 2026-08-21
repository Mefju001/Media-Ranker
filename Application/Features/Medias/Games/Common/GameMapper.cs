using Application.Features.Genres.GetAll;
using Application.Features.Medias.Movies.Common;
using Application.Features.UserInteractions.Reviews.Common;
using Domain.Aggregate;

namespace Application.Features.Medias.Games.Common
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
                game.Details.Developer,
                game.Details.Engine,
                game.PegiRating.AgeValue,
                game.SupportsCrossPlay,
                game.Platforms.Values.ToList()
                );
        }
        public static GameResponse ToGameResponse(Game game, GenreResponse genreResponse)
        {
            return new GameResponse(
                game.Id,
                game.Title,
                game.Description,
                genreResponse,
                game.ReleaseDate.Value,
                game.Language,
                game.Reviews?.Select(r => ReviewMapper.ToResponse(r)).ToList() ?? new List<ReviewResponse>(),
                MediaStatsMapper.ToResponse(game.Stats!) ?? new MediaStatsResponse(0, 0, null),
                game.Details.Developer,
                game.Details.Engine,
                game.PegiRating.AgeValue,
                game.SupportsCrossPlay,
                game.Platforms.Values.ToList()
            );
        }
    }
}
