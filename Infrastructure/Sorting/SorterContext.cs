using Application.Common.Interfaces;

namespace Infrastructure.Sorting
{
    public class SorterContext<T> : ISorterContext<T> where T : class
    {
        private readonly Dictionary<string, ISortingStrategy<T>> strategies;
        public SorterContext(IEnumerable<ISortingStrategy<T>> strategies)
        {
            this.strategies = strategies.ToDictionary(
                s => s.Key,
                s => s);
        }
        public IQueryable<T> Sort(IQueryable<T> query, string? SortByfield, bool isDescending)
        {
            if (string.IsNullOrEmpty(SortByfield)) return query;
            if (strategies.TryGetValue(SortByfield.ToLower(), out var strategy))
            {
                return strategy.ApplySort(query, isDescending);
            }
            return query;
        }
    }
}
