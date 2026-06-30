using Application.Common.Interfaces;
using Application.Features.Common.Notification;
using Application.Features.Games.Command;
using Application.Features.Games.Common;
using Application.Features.Genres.GenreManager;
using Domain.Aggregate;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Extensions;
using Domain.Value_Object;
using MediatR;

namespace Application.Features.Games.AddRange
{
    //maybe add better response with info about which games were added and which not, and why.
    internal class AddRangeHandler : IRequestHandler<AddRangeCommand, List<GameResponse>>
    {
        private readonly IGenreManager genreManager;
        private readonly IMediaRepository<Game> mediaRepository;
        public AddRangeHandler( IGenreManager genreManager, IMediaRepository<Game> mediaRepository)
        {
            this.genreManager = genreManager;
            this.mediaRepository = mediaRepository;
        }
        public async Task<List<GameResponse>> Handle(AddRangeCommand requests, CancellationToken cancellationToken)
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
                        new ReleaseDate(gameReq.ReleaseDate ?? DateTime.UtcNow),
                        genre.id,
                        gameReq.Developer ?? "Unknown",
                        EPlatformExtensions.ToEnum(gameReq.Platforms));
            }).ToList();
            await mediaRepository.AddRangeAsync(games, cancellationToken);
            var genresById = genresDict.Values.ToDictionary(
                g => g.id);
            return games.Select(g => GameMapper.ToGameResponse(g, genresById[g.GenreId])).ToList();
        }
        
    }
}
