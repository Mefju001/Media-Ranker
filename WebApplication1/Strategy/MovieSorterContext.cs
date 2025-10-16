using WebApplication1.Models;
using WebApplication1.Strategy.Interfaces;

namespace WebApplication1.Strategy
{
    public class MovieSorterContext
    {
        private readonly Dictionary<string, ISortingStrategy<Movie>> strategies;
        public MovieSorterContext(IEnumerable<ISortingStrategy<Movie>> strategies)
        {
            this.strategies = strategies.ToDictionary(
                s => s.GetType().Name.ToLower(),
                s => s);
        }
        public IQueryable<Movie> Sort(IQueryable<Movie> query, string sortKey)
        {
            if(string.IsNullOrEmpty(sortKey))return query;
            if (strategies.TryGetValue(sortKey.ToLower(), out var strategy))
            {
                return strategy.ApplySort(query);
            }
            return query;
        }
    }
}
