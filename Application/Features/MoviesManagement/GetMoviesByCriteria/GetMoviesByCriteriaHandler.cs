using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Mapper;
using WebApplication1.Domain.Entities;
using WebApplication1.Infrastructure.Persistence;
using WebApplication1.Infrastructure.Sorting;
using WebApplication1.Infrastructure.Specification;
using WebApplication1.QueryHandler;

namespace WebApplication1.Application.Features.Movies.GetMoviesByCriteria
{
    public class GetMoviesByCriteriaHandler : IRequestHandler<GetMoviesByCriteriaQuery, List<MovieResponse>>
    {
        private readonly IAppDbContext context;
        private readonly ISorterContext<Movie> sorterContext;

        public GetMoviesByCriteriaHandler(ISorterContext<Movie> sorterContext, IAppDbContext appDbContext)
        {
            this.sorterContext = sorterContext;
            this.context = appDbContext;
        }


        public async Task<List<MovieResponse>> Handle(GetMoviesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = context.Movies
                .Include(m => m.genre)
                .Include(m => m.director)
                .Include(m => m.Stats)
                .AsNoTracking()
                .AsQueryable();
            //IQueryable<Movie> query = queryServices.StartQuery();
            var predicate = BuildPredicate(request);
            query = Filter(query, predicate);
            if (!string.IsNullOrEmpty(request.SortByField) && request.SortByField.Contains('|'))
            {
                var fields = request.SortByField.Split('|');
                if (fields.Length == 2 && bool.TryParse(fields[1], out bool IsDescending))
                {
                    request.IsDescending = IsDescending;
                    request.SortByField = fields[0];
                    query = sorterContext.Sort(query, request.SortByField, request.IsDescending);
                }
                else throw new Exception("Mismatch");
            }
            var movies = await query.ToListAsync(cancellationToken);
            var Response = movies.Select(m => MovieMapper.ToMovieResponse(m)).ToList();
            return Response;
        }
        private Expression<Func<Movie, bool>> BuildPredicate(GetTvSeriesByCriteriaQuery query)
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
                finalPredicate = finalPredicate.And(m => m.Stats!.AverageRating >= query.MinRating.Value);
            }
            if (query.ReleaseYear.HasValue)
            {
                finalPredicate = finalPredicate.And(m => m.ReleaseDate.Year == query.ReleaseYear);
            }
            return finalPredicate;
        }

    }
}
