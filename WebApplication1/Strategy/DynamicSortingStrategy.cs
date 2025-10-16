using System.Linq.Expressions;
using WebApplication1.Models;
using WebApplication1.Strategy.Interfaces;

namespace WebApplication1.Strategy
{
    public class DynamicSortingStrategy<T> : ISortingStrategy<T> where T : class
    {
        private readonly Expression<Func<T, object>> sortExpression;
        private readonly bool isDescending;
        public DynamicSortingStrategy(Expression<Func<T, object>> sortExpression, bool isDescending)
        {
            this.sortExpression = sortExpression;
            this.isDescending = isDescending;
        }

        public IQueryable<T> ApplySort(IQueryable<T> query)
        {
            if (isDescending)
                return query.OrderByDescending(sortExpression);
            return query.OrderBy(sortExpression);
        }
    }
}
