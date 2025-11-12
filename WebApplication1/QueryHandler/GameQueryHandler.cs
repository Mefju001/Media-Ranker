using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplication1.DTO.Mapping;
using WebApplication1.DTO.Request;
using WebApplication1.DTO.Response;
using WebApplication1.Models;
using WebApplication1.QueryHandler.Query;
using WebApplication1.Services;
using WebApplication1.Specification;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.QueryHandler
{
    public class GameQueryHandler(QueryServices<Game>queryServices):IRequestHandler<GameQuery, List<GameResponse>>
    {
        private readonly QueryServices<Game> gameServices = queryServices;

        public async Task<List<GameResponse>> Handle(GameQuery request, CancellationToken cancellationToken)
        {
            var query = gameServices.StartQuery();
            var predicate = buildPredicate(request);
            query = gameServices.Filter(query, predicate);
            if (!string.IsNullOrEmpty(request.sortByField) || request.IsDescending)
            {
                query = gameServices.Sort(query, request.sortByField, request.IsDescending);
            }

            var games = await query.ToListAsync(cancellationToken);
            return games.Select(x=>GameMapping.ToGameResponse(x)).ToList();
        }
        private Expression<Func<Game, bool>> buildPredicate(GameQuery gameQuery)
        {
            var finalPredicate = PredicateBuilder.True<Game>();
            if (!string.IsNullOrEmpty(gameQuery.title))
            {
                finalPredicate = finalPredicate.And(m => m.title.Contains(gameQuery.title));
            }
            if (!string.IsNullOrEmpty(gameQuery.genreName))
            {
                finalPredicate = finalPredicate.And(m => m.genre.name.Contains(gameQuery.genreName));
            }
            /*if (query.MinRating.HasValue)
            {
                finalPredicate = finalPredicate.And(m => m.Reviews.Average(x => (double?)x.Rating) >= query.MinRating.Value);
            }
            if (query.ReleaseYear.HasValue)
            {
                finalPredicate = finalPredicate.And(m => m.ReleaseDate.Year == query.ReleaseYear);
            }*/
            return finalPredicate;
        
        }
    }
}
