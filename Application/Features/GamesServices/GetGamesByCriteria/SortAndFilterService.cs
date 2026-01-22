using Application.Common.Interfaces;
using Application.Features.MovieServices.GetMoviesByCriteria;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.GamesServices.GetGamesByCriteria
{
    internal class SortAndFilterService
    {
        public static IQueryable<GameDomain> ApplyFilters(IQueryable<GameDomain> query, GetGamesByCriteriaQuery request)
        {
            if (!string.IsNullOrWhiteSpace(request.title))
            {
                query = query.Where(m => m.Title.Contains(request.title));
            }
            if (!string.IsNullOrWhiteSpace(request.genreName))
            {
                query = query.Where(m => m.GenreDomain.name.Contains(request.genreName));
            }
            if (request.MinRating.HasValue)
            {
                query = query.Where(m => m.Stats!.AverageRating >= request.MinRating);
            }
            if (request.releaseDate.HasValue)
            {
                query = query.Where(m => m.ReleaseDate.Year == request.releaseDate);
            }
            if (!string.IsNullOrWhiteSpace(request.developer))
            {
                query = query.Where(m => m.Developer == request.developer);
            }
            return query;
        }
        public static IQueryable<GameDomain> ApplySorting(IQueryable<GameDomain> query, GetGamesByCriteriaQuery request)
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
        private static Dictionary<string, Expression<Func<GameDomain, object>>> DictionaryOfSortAbility()
        {
            var columns = new Dictionary<string, Expression<Func<GameDomain, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                ["Title"] = m => m.Title,
                ["Rating"] = m => m.Stats.AverageRating!,
                ["Date"] = m => m.ReleaseDate,
            };
            return columns;
        }
    }
}
}
