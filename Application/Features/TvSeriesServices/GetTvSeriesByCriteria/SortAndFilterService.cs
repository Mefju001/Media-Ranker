using Domain.Entity;
using System.Linq.Expressions;

namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    internal class SortAndFilterService
    {
        public static IQueryable<TvSeriesDomain> ApplyFilters(IQueryable<TvSeriesDomain> query, GetTvSeriesByCriteriaQuery request)
        {
            if (!string.IsNullOrWhiteSpace(request.TitleSearch))
            {
                query = query.Where(m => m.Title.Contains(request.TitleSearch));
            }
            if (!string.IsNullOrWhiteSpace(request.genreName))
            {
                query = query.Where(m => m.GenreDomain.name.Contains(request.genreName));
            }
            if (request.MinRating.HasValue)
            {
                query = query.Where(m => m.Stats!.AverageRating >= request.MinRating);
            }
            if (request.ReleaseYear.HasValue)
            {
                query = query.Where(m => m.ReleaseDate.Year == request.ReleaseYear);
            }
            if (!string.IsNullOrWhiteSpace(request.network))
            {
                query = query.Where(m => m.Status.ToString() == request.network);
            }
            return query;
        }
        public static IQueryable<TvSeriesDomain> ApplySorting(IQueryable<TvSeriesDomain> query, GetTvSeriesByCriteriaQuery request)
        {
            if (!string.IsNullOrEmpty(request.SortByField))
            {
                var sortAbility = DictionaryOfSortAbility();
                sortAbility.TryGetValue(request.SortByField, out var sortExpression);
                if (sortExpression == null) return query;
                if (request.IsDescending)
                    return query.OrderByDescending(sortExpression);
                return query.OrderBy(sortExpression);
            }
            return query;
        }
        private static Dictionary<string, Expression<Func<TvSeriesDomain, object>>> DictionaryOfSortAbility()
        {
            var columns = new Dictionary<string, Expression<Func<TvSeriesDomain, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                ["Title"] = m => m.Title,
                ["Rating"] = m => m.Stats.AverageRating!,
                ["Date"] = m => m.ReleaseDate,
            };
            return columns;
        }
    }
}
