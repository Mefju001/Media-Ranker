using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;
using WebApplication1.Strategy;

namespace WebApplication1.QueryHandler
{
    public class QueryService<T>where T:class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly SorterContext<T> _sorterContext;

        public QueryService(SorterContext<T> sorterContext, IGenericRepository<T> repository)
        {
            _repository = repository;
            _sorterContext = sorterContext;
        }
        public IQueryable<T>Handle(Expression<Func<T,bool>> filterPredicate, string SortByfield,bool SortByDirector)
        {
            IQueryable<T> query = _repository.AsQueryable();
            if (filterPredicate != null)
            {
                query = query.Where(filterPredicate);
            }
            if (!string.IsNullOrEmpty(SortByfield) || SortByDirector)
            {
                query = _sorterContext.Sort(query, SortByfield, SortByDirector);
            }
            return query;
        }
    }
}
