using System.Linq.Expressions;
using WebApplication1.Infrastructure.Sorting;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.QueryHandler
{
    public class QueryServices<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly SorterContext<T> _sorterContext;

        public QueryServices(SorterContext<T> sorterContext, IGenericRepository<T> repository)
        {
            _sorterContext = sorterContext;
            _repository = repository;
        }
        public IQueryable<T> StartQuery()
        {
            return _repository.AsQueryable();
        }
        public IQueryable<T> Filter(IQueryable<T> query, Expression<Func<T, bool>> filterPredicate)
        {
            query = query.Where(filterPredicate);
            return query;
        }
        public IQueryable<T> Sort(IQueryable<T> query, string SortByField, bool isDescending)
        {
            query = _sorterContext.Sort(query, SortByField, isDescending);
            return query;
        }
    }
}
