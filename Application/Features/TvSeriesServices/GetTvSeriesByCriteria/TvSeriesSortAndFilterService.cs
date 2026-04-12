using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public class TvSeriesSortAndFilterService : ITvSeriesSortAndFilterService
    {
        private readonly IAppDbContext appDbContext;
        public TvSeriesSortAndFilterService(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
        public async Task<List<TvSeriesResponse>> Handler(GetTvSeriesByCriteriaQuery request, CancellationToken ct)
        {
            var query = appDbContext.Set<TvSeries>().AsNoTrackingWithIdentityResolution().AsSplitQuery();
            query = ApplyFilters(query, request);
            query = ApplySorting(query, request);
            return await query
                .Join(appDbContext.Set<Genre>(), tv => tv.GenreId, gen => gen.Id, (tv, gen) => new { tv, gen })
                .Select(x => TvSeriesMapper.ToTvSeriesResponse(x.tv, x.gen))
                .ToListAsync(ct);
        }
        private IQueryable<TvSeries> ApplyFilters(IQueryable<TvSeries> query, GetTvSeriesByCriteriaQuery request)
        {
            if (!string.IsNullOrWhiteSpace(request.TitleSearch))
            {
                query = query.Where(m => m.Title.Contains(request.TitleSearch));
            }
            if (!string.IsNullOrWhiteSpace(request.genreName))
            {
                var genreIds = appDbContext.Set<Genre>()
                                    .Where(gen => gen.Name.Value.Contains(request.genreName))
                                    .Select(gen => gen.Id);

                query = query.Where(g => genreIds.Contains(g.GenreId));
            }
            if (request.MinRating.HasValue)
            {
                query = query.Where(m => m.Stats!.AverageRating >= request.MinRating);
            }
            if (request.ReleaseYear.HasValue)
            {
                query = query.Where(m => m.ReleaseDate!.Value.Year == request.ReleaseYear);
            }
            if (!string.IsNullOrWhiteSpace(request.network))
            {
                query = query.Where(m => m.Status.ToString() == request.network);
            }
            return query;
        }
        private IQueryable<TvSeries> ApplySorting(IQueryable<TvSeries> query, GetTvSeriesByCriteriaQuery request)
        {
            var sortField = request.SortByField;
            var isDescending = request.IsDescending;

            if (!string.IsNullOrEmpty(sortField) && sortField.Contains('|'))
            {
                var parts = sortField.Split('|');
                sortField = parts[0];
                isDescending = parts.Length > 1 && parts[1].ToLower() != "false";
            }

            if (!string.IsNullOrEmpty(sortField) && sortColumns.TryGetValue(sortField, out var sortExp))
            {
                return isDescending ? query.OrderByDescending(sortExp) : query.OrderBy(sortExp);
            }

            return query.OrderBy(g => g.Title);
        }
        private static readonly Dictionary<string, Expression<Func<TvSeries, object>>> sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Title"] = m => m.Title,
                ["Rating"] = m => m.Stats.AverageRating,
                ["Date"] = m => m.ReleaseDate!.Value,
            };
    }
}
