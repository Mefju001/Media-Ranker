using Domain.Aggregate;
using Domain.Enums;
using System.Linq.Expressions;
using domain = Domain.Aggregate;
namespace Domain.Specification
{
    public class TvSeriesFilterSpecification:BaseSpecification<TvSeries>
    {
        private static readonly Dictionary<string, Expression<Func<domain.TvSeries, object>>> sortColumns =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["Title"] = m => m.Title,
                ["Rating"] = m => m.Stats.AverageRating,
                ["Date"] = m => m.ReleaseDate!.Value,
            };
        public TvSeriesFilterSpecification(string? TitleSearch,
        double? MinRating, int? ReleaseYear, List<Guid>? genreIds,
        int? seasons, int? episodes, string? network, ETvSeriesStatus? status,
        string? SortByField, bool IsDescending)
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
            if (ReleaseYear.HasValue)
            {
                var year = ReleaseYear.Value;
                var startOfYear = new DateTime(year, 1, 1);
                var endOfYear = new DateTime(year, 12, 31, 23, 59, 59);
                AddCriteria(m => m.ReleaseDate >= startOfYear && m.ReleaseDate <= endOfYear);
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
