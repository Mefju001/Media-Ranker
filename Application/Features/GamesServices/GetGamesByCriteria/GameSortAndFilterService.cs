using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public class GameSortAndFilterService : IGameSortAndFilterService
    {
        private readonly IAppDbContext appDbContext;
        public GameSortAndFilterService(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<GameResponse>> GetGamesByCriteriaAsync(GetGamesByCriteriaQuery request, CancellationToken ct)
        {
            var query = appDbContext.Set<Game>().AsNoTrackingWithIdentityResolution().AsSplitQuery();

            query = ApplyFilters(query, request);

            query = ApplySorting(query, request);

            return await query
                .Join(appDbContext.Set<Genre>(), g => g.GenreId, gen => gen.Id, (g, gen) => new { g, gen })
                .Select(x => new GameResponse(
                    x.g.Id,
                    x.g.Title,
                    x.g.Description,
                    new GenreResponse(x.g.GenreId, x.gen.Name),
                    x.g.ReleaseDate,
                    x.g.Language,
                    x.g.Reviews.Select(r => new ReviewResponse(
                        r.Id,
                        r.MediaId,
                        r.Username,
                        r.Rating,
                        r.Comment,
                        r.AuditInfo != null ? r.AuditInfo.CreatedAt : DateTime.UtcNow,
                        r.AuditInfo != null ? r.AuditInfo.UpdatedAt : null
                    )).ToList(),
                    new MediaStatsResponse(
                        x.g.Stats.AverageRating,
                        x.g.Stats.ReviewCount,
                        x.g.Stats.LastCalculated
                    ),
                    x.g.Developer,
                    x.g.Platform
                ))
                .ToListAsync(ct);
        }

        private IQueryable<Game> ApplyFilters(IQueryable<Game> query, GetGamesByCriteriaQuery request)
        {
            if (!string.IsNullOrWhiteSpace(request.title))
                query = query.Where(m => m.Title.Contains(request.title));

            if (!string.IsNullOrWhiteSpace(request.genreName))
            {
                var genreIds = appDbContext.Set<Genre>()
                    .Where(gen => gen.Name.Value.Contains(request.genreName))
                    .Select(gen => gen.Id);

                query = query.Where(g => genreIds.Contains(g.GenreId));
            }

            if (request.MinRating.HasValue)
                query = query.Where(m => m.Stats.AverageRating >= request.MinRating);

            if (request.releaseDate.HasValue)
                query = query.Where(m => m.ReleaseDate.Value.Year == request.releaseDate);

            return query;
        }

        private IQueryable<Game> ApplySorting(IQueryable<Game> query, GetGamesByCriteriaQuery request)
        {
            var sortField = request.sortByField;
            var isDescending = request.IsDescending;

            if (!string.IsNullOrEmpty(sortField) && sortField.Contains('|'))
            {
                var parts = sortField.Split('|');
                sortField = parts[0];
                isDescending = parts.Length > 1 && parts[1].ToLower() != "false";
            }

            if (!string.IsNullOrEmpty(sortField) && _sortColumns.TryGetValue(sortField, out var sortExp))
            {
                return isDescending ? query.OrderByDescending(sortExp) : query.OrderBy(sortExp);
            }

            return query.OrderBy(g => g.Title);
        }

        private static readonly Dictionary<string, Expression<Func<Game, object>>> _sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Title"] = g => g.Title,
                ["Rating"] = g => g.Stats.AverageRating,
                ["Date"] = g => g.ReleaseDate.Value
            };
    }
}

