using Domain.Entity;
using System.Linq.Expressions;

namespace Application.Features.MovieServices.GetMoviesByCriteria
{
    internal static class SortAndFilterService
    {
        public static IQueryable<MovieDomain> ApplyFilters(IQueryable<MovieDomain> query, GetMoviesByCriteriaQuery request)
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
            if (!string.IsNullOrWhiteSpace(request.DirectorSurname)&& !string.IsNullOrWhiteSpace(request.DirectorSurname))
            { 
                query = query.Where(m => m.DirectorDomain.name.Contains(request.DirectorName!)&&m.DirectorDomain.surname.Contains(request.DirectorSurname!));
            }
            return query;
        }
        public static IQueryable<MovieDomain> ApplySorting(IQueryable<MovieDomain> query, GetMoviesByCriteriaQuery request)
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
        private static Dictionary<string, Expression<Func<MovieDomain, object>>> DictionaryOfSortAbility()
        {
            var columns = new Dictionary<string, Expression<Func<MovieDomain, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                ["Title"] = m => m.Title,
                ["Rating"] = m => m.Stats.AverageRating!,
                ["Date"] = m => m.ReleaseDate,
                ["Director"] = m => m.DirectorId
            };
            return columns;
        }
    }
}
