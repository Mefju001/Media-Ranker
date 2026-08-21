using Application.Features.Genres.GenreManager;
using Domain.Aggregate;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Repository;
using Domain.Value_Object;
using MediatR;

namespace Application.Features.Medias.Games.AddRange
{
    internal class AddRangeHandler : IRequestHandler<AddRangeCommand, List<Guid>>
    {
        private readonly IGenreManager genreManager;
        private readonly IMediaRepository<Game> mediaRepository;
        public AddRangeHandler( IGenreManager genreManager, IMediaRepository<Game> mediaRepository)
        {
            this.genreManager = genreManager;
            this.mediaRepository = mediaRepository;
        }
        public async Task<List<Guid>> Handle(AddRangeCommand requests, CancellationToken cancellationToken)
        {
            if (requests.games == null || !requests.games.Any()) return [];
            if (requests.games.Count > 500)
                throw new BadRequestException("The package is too large. Maximum 500 games at a time.");
            var names = requests.games.Select(g => g.Genre.name).Distinct().ToList();
            var genresMap = await genreManager.GetOrCreateBatchAsync(names, cancellationToken);
            var genresDict = genresMap.ToDictionary(g => g.Name, g => g);
            var games = requests.games.Select(gameReq =>
            {
                var genre = genresDict[gameReq.Genre.name];
                return Game.Create(
                        gameReq.Title,
                        gameReq.Description,
                        gameReq.Language,
                        new ReleaseDate(gameReq.ReleaseDate!.Value),
                        genre.id,
                        new GameDetails(gameReq.Developer, gameReq.Engine),
                        gameReq.PegiRating,
                        EPlatformExtensions.ToEnum(gameReq.Platforms),
                        EGameStatusExtensions.ToEnum(gameReq.GameStatus),
                        gameReq.SupportsCrossPlay);
            }).ToList();
            await mediaRepository.AddRangeAsync(games, cancellationToken);
            return games.Select(g => g.Id).ToList();
        }
        
    }
}
