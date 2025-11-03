using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using WebApplication1.Data;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
using WebApplication1.QueryHandler.Query;
using WebApplication1.Services.Interfaces;
using WebApplication1.Strategy;

namespace WebApplication1.QueryHandler
{
    public class QueryHandler<T>where T: class :IRequestHandler<T, List<MovieResponse>>
    {
        private readonly QueryServices<T> queryServices;

        public QueryHandler(QueryServices<T> queryServices)
        {
            this.queryServices = queryServices;
        }


        public IQueryable<T>Handle(MoviesQuery request)
        {
            IQueryable<T> query = _repository.AsQueryable();
            if (request.TitleSearch != null)
            {
                query = query.Where();
            }
            if (!string.IsNullOrEmpty(request.SortByField) || request.IsDescending)
            {

                query = queryServices.Sort(request.SortByField, request.IsDescending);
            }
            return query;
        }
    }
}
