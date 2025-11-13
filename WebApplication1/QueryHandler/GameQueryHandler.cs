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
    public class GameQueryHandler(QueryServices<Game>queryServices):IRequestHandler<GameQuery, List<GameResponse>>
    {
        private readonly QueryServices<Game> gameServices = queryServices;

        public async Task<List<GameResponse>> Handle(GameQuery request, CancellationToken cancellationToken)
        {
            var query = gameServices.StartQuery();
            var predicate = buildPredicate(request);
            query = gameServices.Filter(query, predicate);
            if (!string.IsNullOrEmpty(request.sortByField) || request.IsDescending || request.isAVG)
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
            if (!string.IsNullOrEmpty(gameQuery.platform))
            {
                finalPredicate = finalPredicate.And(m => m.Platform.ToString().Contains(gameQuery.platform));
            }
            if (!string.IsNullOrEmpty(gameQuery.developer))
            {
                finalPredicate = finalPredicate.And(m => m.Developer.Contains(gameQuery.developer));
            }
            return finalPredicate;
        
        }
    }
}
