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

namespace WebApplication1.Application.Features.Games.GetMoviesByCriteria
{
    public class GetGamesByCriteriaHandler : IRequestHandler<GetGamesByCriteriaQuery, List<GameResponse>>
    {
        private readonly AppDbContext context;
        private readonly SorterContext<Game> sorterContext;
        private readonly QueryServices<Game> queryServices;

        public GetGamesByCriteriaHandler(SorterContext<Game> sorterContext, QueryServices<Game> queryServices, AppDbContext appDbContext)
        {
            this.sorterContext = sorterContext;
            this.queryServices = queryServices;
            this.context = appDbContext;
        }


        public async Task<List<GameResponse>> Handle(GetGamesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = context.Games
                .Include(m => m.genre)
                .Include(m => m.Stats)
                .AsNoTracking()
                .AsQueryable();
            //IQueryable<Movie> query = queryServices.StartQuery();
            var predicate = BuildPredicate(request);
            query = Filter(query, predicate);
            if (!string.IsNullOrEmpty(request.sortByField) && request.sortByField.Contains('|'))
            {
                var fields = request.sortByField.Split('|');
                if (fields.Length == 2 && bool.TryParse(fields[1], out bool IsDescending))
                {
                    request.IsDescending = IsDescending;
                    request.sortByField = fields[0];
                    query = sorterContext.Sort(query, request.sortByField, request.IsDescending);
                }
                else throw new Exception("Mismatch");
            }
            var movies = await query.ToListAsync(cancellationToken);
            var Response = movies.Select(m => MovieMapper.ToMovieResponse(m)).ToList();
            return Response;
        }
        private Expression<Func<Game, bool>> BuildPredicate(GetGamesByCriteriaQuery query)
        {
            var finalPredicate = PredicateBuilder.True<Game>();
            if (!string.IsNullOrEmpty(query.title))
            {
                finalPredicate = finalPredicate.And(g => g.title.Contains(query.title));
            }
            if (!string.IsNullOrEmpty(query.genreName))
            {
                finalPredicate = finalPredicate.And(g => g.genre.name.Contains(query.genreName));
            }
            if (query.MinRating.HasValue)
            {
                finalPredicate = finalPredicate.And(g => g.Stats!.AverageRating >= query.MinRating.Value);
            }
            if (query.releaseDate.HasValue)
            {
                finalPredicate = finalPredicate.And(g => g.ReleaseDate.Year == query.releaseDate);
            }
            return finalPredicate;
        }
        private IQueryable<Game> Filter(IQueryable<Game> query, Expression<Func<Game, bool>> filterPredicate)
        {
            query = query.Where(filterPredicate);
            return query;
        }
    }
}
