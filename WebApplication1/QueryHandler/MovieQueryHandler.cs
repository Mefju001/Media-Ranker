using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
using WebApplication1.QueryHandler.Query;
using WebApplication1.Specification;

namespace WebApplication1.QueryHandler
{
    public class MovieQueryHandler : IRequestHandler<MovieQuery, List<MovieResponse>>
    {
        private readonly QueryServices<Movie> queryServices;

        public MovieQueryHandler(QueryServices<Movie> queryServices)
        {
            this.queryServices = queryServices;
        }


        public async Task<List<MovieResponse>> Handle(MovieQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Movie> query = queryServices.StartQuery();
            var predicate = BuildPredicate(request);
            query = queryServices.Filter(query, predicate);
            if (!string.IsNullOrEmpty(request.SortByField) || request.IsDescending)
            {
                query = queryServices.Sort(query, request.SortByField, request.IsDescending);
            }
            var movies = await query.ToListAsync(cancellationToken);
            var Response = movies.Select(m => MovieMapper.ToMovieResponse(m)).ToList();
            return Response;
        }
        private Expression<Func<Movie, bool>> BuildPredicate(MovieQuery query)
        {
            var finalPredicate = PredicateBuilder.True<Movie>();
            if (!string.IsNullOrEmpty(query.TitleSearch))
            {
                finalPredicate = finalPredicate.And(m => m.title.Contains(query.TitleSearch));
            }
            if (!string.IsNullOrEmpty(query.genreName))
            {
                finalPredicate = finalPredicate.And(m => m.genre.name.Contains(query.genreName));
            }
            if (query.MinRating.HasValue)
            {
                finalPredicate = finalPredicate.And(m => m.Reviews.Average(x => (double?)x.Rating) >= query.MinRating.Value);
            }
            if (query.ReleaseYear.HasValue)
            {
                finalPredicate = finalPredicate.And(m => m.ReleaseDate.Year == query.ReleaseYear);
            }
            return finalPredicate;
        }
    }
}
