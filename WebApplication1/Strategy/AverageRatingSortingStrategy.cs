using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplication1.Models;
using WebApplication1.Strategy.Interfaces;

namespace WebApplication1.Strategy
{
    public class AverageRatingSortingStrategy : ISortingStrategy<Movie>
    {
        public string Key { get; }
        public AverageRatingSortingStrategy(string key)
        {
            Key = key;
        }

        public IQueryable<Movie> ApplySort(IQueryable<Movie> query, bool isDescending)
        {
            Expression<Func<Movie, double>> sortExpression 
                = m => m.Reviews.Average(x => (double?)x.Rating) ?? 0;
            if (isDescending)
            {
                return query.OrderByDescending(sortExpression);
            }
            return query.OrderBy(sortExpression);
        }
    }
}
