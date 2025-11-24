using System.Linq.Expressions;

namespace WebApplication1.Strategy
{
    public class DynamicSortingStrategy<T> : ISortingStrategy<T> where T : class
    {
        public string Key { get; }
        private readonly Expression<Func<T, object>> sortExpression;
        public DynamicSortingStrategy(string key, Expression<Func<T, object>> sortExpression)
        {
            Key = key.ToLower();
            this.sortExpression = sortExpression;
        }

        public IQueryable<T> ApplySort(IQueryable<T> query, bool isDescending)
        {
            if (isDescending)
                return query.OrderByDescending(sortExpression);
            return query.OrderBy(sortExpression);
        }
    }
}
