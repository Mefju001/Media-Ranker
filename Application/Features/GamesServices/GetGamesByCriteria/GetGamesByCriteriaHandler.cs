using Application.Common.Interfaces;
using Application.Features.GamesManagement.GetGamesByCriteria;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Mapper;
using WebApplication1.Domain.Entities;


namespace Application.Features.GamesServices.GetGamesByCriteria
{
    public class GetGamesByCriteriaHandler : IRequestHandler<GetGamesByCriteriaQuery, List<GameResponse>>
    {
        private readonly IAppDbContext context;
        private readonly ISorterContext<Game> sorterContext;
        private readonly IGameBuildPredicate gameBuildPredicate;
        private readonly IGameFilter gameFilter;

        public GetGamesByCriteriaHandler(ISorterContext<Game> sorterContext, IAppDbContext appDbContext, IGameFilter gameFilter, IGameBuildPredicate gameBuildPredicate)
        {
            this.sorterContext = sorterContext;
            this.context = appDbContext;
            this.gameFilter = gameFilter;
            this.gameBuildPredicate = gameBuildPredicate;
        }


        public async Task<List<GameResponse>> Handle(GetGamesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = context.Games
                .Include(m => m.genre)
                .Include(m => m.Stats)
                .AsNoTracking()
                .AsQueryable();
            var predicate = gameBuildPredicate.BuildPredicate(request);
            query = gameFilter.Filter(query, predicate);
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
            var Response = movies.Select(m => GameMapper.ToGameResponse(m)).ToList();
            return Response;
        }
    }
}
