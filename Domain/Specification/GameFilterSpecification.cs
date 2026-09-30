using Domain.Aggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Domain.Specification
{
    public class GameFilterSpecification : BaseSpecification<Game>
    {
        private static readonly Dictionary<string, Expression<Func<Game, object>>> sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Title"] = g => g.Title,
                ["Rating"] = g => g.Stats.AverageRating,
                ["Date"] = g => g.ReleaseDate.Value
            };
        public GameFilterSpecification(
            string? TitleSearch,
            List<Guid>? genreIds,
            string? platform,
            string? developer,
            int? ReleaseYear,
            int? MinRating,
            string? SortByField,
            bool IsDescending
            )
        {
            if (!string.IsNullOrWhiteSpace(TitleSearch))
            {
                AddCriteria(m => m.Title.Contains(TitleSearch));
            }
            if (genreIds != null && genreIds.Any())
            {
                AddCriteria(m => genreIds.Contains(m.GenreId));
            }
            if (MinRating.HasValue)
            {
                AddCriteria(m => m.Stats!.AverageRating >= MinRating);
            }
            if(!string.IsNullOrWhiteSpace(developer))
            {
                AddCriteria(g => g.Details.Developer.Contains(developer));
            }
            if (!string.IsNullOrWhiteSpace(platform))
            {
                var searchPlatform = "%" + platform + "%";
                AddCriteria(g => EF.Functions.Like((string)(object)g.Platforms,searchPlatform));
            }
            if (ReleaseYear.HasValue)
            {
                var year = ReleaseYear.Value;
                var startOfYear = new DateTime(year, 1, 1);
                var endOfYear = new DateTime(year, 12, 31, 23, 59, 59);
                AddCriteria(m => m.ReleaseDate.Value >= startOfYear && m.ReleaseDate.Value <= endOfYear);
            }
            if (!string.IsNullOrWhiteSpace(SortByField) && sortColumns.TryGetValue(SortByField, out var sortExpression))
            {
                if (IsDescending) ApplyOrderByDescending(sortExpression);
                else ApplyOrderBy(sortExpression);
            }
            else { ApplyOrderBy(m => m.Title); }
        }
    }
}
