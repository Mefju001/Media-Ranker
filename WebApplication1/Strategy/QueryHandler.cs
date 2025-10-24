using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Strategy
{
    public class QueryHandler<T>where T:class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly SorterContext<T> _sorterContext;

        public QueryHandler(SorterContext<T> sorterContext, IGenericRepository<T> repository)
        {
            _repository = repository;
            _sorterContext = sorterContext;
        }
        public IQueryable<T>Handle(string SortByfield,bool SortByDirector)
        {
            IQueryable<T> query = _repository.AsQueryable();
            query = _sorterContext.Sort(query, SortByfield,SortByDirector);
            return query.AsQueryable();
        }
    }
}
