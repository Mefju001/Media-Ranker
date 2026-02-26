using Application.Common.Interfaces;
using Domain.Entity;
using System.Linq.Expressions;

namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public class GameSortAndFilterService : IGameSortAndFilterService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IGameRepository gameRepository;
        private readonly IGenreRepository genreRepository;
        public GameSortAndFilterService(IUnitOfWork unitOfWork, IGameRepository gameRepository, IGenreRepository genreRepository)
        {
            this.unitOfWork = unitOfWork;
            this.gameRepository = gameRepository;
            this.genreRepository = genreRepository;
        }
        public async Task<IQueryable<Game>> GetGamesByCriteriaAsync(GetGamesByCriteriaQuery request)
        {
            var filteredGames = await ApplyFiltersAsync(request);
            var sortedGames = await ApplySorting(filteredGames, request);
            return sortedGames;
        }
        private async Task<IQueryable<Game>> ApplyFiltersAsync(GetGamesByCriteriaQuery request)
        {
            var query = await gameRepository.AsQueryable();
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
                query = query.Where(m => m.ReleaseDate.Value.Year == request.releaseDate);
            }
            if (!string.IsNullOrWhiteSpace(request.developer))
            {
                query = query.Where(m => m.Developer == request.developer);
            }
            return query;
        }
        private async Task<IQueryable<Game>> ApplySorting(IQueryable<Game> query, GetGamesByCriteriaQuery request)
        {
            if (!string.IsNullOrEmpty(request.sortByField))
            {
                var sortAbility = DictionaryOfSortAbility();
                sortAbility.TryGetValue(request.sortByField, out var sortExpression);
                if (sortExpression == null) return query;
                if (request.IsDescending)
                    return query.OrderByDescending(sortExpression);
                return query.OrderBy(sortExpression);
            }
            return query;
        }
        private static Dictionary<string, Expression<Func<Game, object>>> DictionaryOfSortAbility()
        {
            var columns = new Dictionary<string, Expression<Func<Game, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                ["Title"] = m => m.Title,
                ["Rating"] = m => m.Stats.AverageRating!,
                ["Date"] = m => m.ReleaseDate,
            };
            return columns;
        }
    }
}

