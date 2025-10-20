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
                s => s.Key,
                s => s);
        }
        public IQueryable<Movie> Sort(IQueryable<Movie> query, string SortByfield, bool SortByDirector)
        {
            if(string.IsNullOrEmpty(SortByfield))return query;
            if (strategies.TryGetValue(SortByfield.ToLower(), out var strategy))
            {
                return strategy.ApplySort(query,SortByDirector);
            }
            return query;
        }
    }
}
