using Application.Common.Interfaces;
using Domain.Entity;
using System.Linq.Expressions;

namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public class GameSortAndFilterService : IGameSortAndFilterService
    {

        private readonly IGameRepository gameRepository;
        private readonly IGenreRepository genreRepository;
        public GameSortAndFilterService(IGameRepository gameRepository, IGenreRepository genreRepository)
        {
            this.gameRepository = gameRepository;
            this.genreRepository = genreRepository;
        }
        public IQueryable<Game> GetGamesByCriteriaAsync(GetGamesByCriteriaQuery request)
        {
            var filteredGames = ApplyFiltersAsync(request);
            var sortedGames = ApplySorting(filteredGames, request);
            return sortedGames;
        }
        private IQueryable<Game> ApplyFiltersAsync(GetGamesByCriteriaQuery request)
        {
            var query = gameRepository.AsQueryable();
            if (!string.IsNullOrWhiteSpace(request.title))
            {
                query = query.Where(m => m.Title.Contains(request.title));
            }
            if (!string.IsNullOrWhiteSpace(request.genreName))
            {
                var genreTable = genreRepository.GetAllQueryable();
                query = query.Join(genreTable,
                    game => game.GenreId,
                    genre => genre.Id,
                    (game, genre) => new { game, genre }
                    )
                    .Where(ge => ge.genre.name.Value.Contains(request.genreName))
                    .Select(ge => ge.game);
            }
            if (request.MinRating.HasValue)
            {
                query = query.Where(m => m.Stats!.AverageRating >= request.MinRating);
            }
            if (request.releaseDate.HasValue)
            {
                query = query.Where(m => m.ReleaseDate!.Value.Year == request.releaseDate);
            }
            if (!string.IsNullOrWhiteSpace(request.developer))
            {
                query = query.Where(m => m.Developer == request.developer);
            }
            return query;
        }
        private IQueryable<Game> ApplySorting(IQueryable<Game> query, GetGamesByCriteriaQuery request)
        {
            if (!string.IsNullOrEmpty(request.sortByField) && sortColumns.TryGetValue(request.sortByField, out var sortExpression))
            {
                return request.IsDescending
                    ? query.OrderByDescending(sortExpression)
                    : query.OrderBy(sortExpression);
            }
            return query;
        }
        private static readonly Dictionary<string, Expression<Func<Game, object>>> sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Title"] = m => m.Title,
                ["Rating"] = m => m.Stats.AverageRating!,
                ["Date"] = m => m.ReleaseDate!,
            };
    }
}

