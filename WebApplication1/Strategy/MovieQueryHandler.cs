using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services.Interfaces;

namespace WebApplication1.Strategy
{
    public class MovieQueryHandler
    {
        private readonly IGenericRepository<Movie> _repository;
        private readonly MovieSorterContext _sorterContext;

        public MovieQueryHandler(MovieSorterContext sorterContext, IGenericRepository<Movie> repository)
        {
            _repository = repository;
            _sorterContext = sorterContext;
        }
        public IQueryable<Movie>Handle(string SortByfield,bool SortByDirector)
        {
            IQueryable<Movie> query = _repository.AsQueryable();
            query = _sorterContext.Sort(query, SortByfield,SortByDirector);
            return query.AsQueryable();
        }
    }
}
