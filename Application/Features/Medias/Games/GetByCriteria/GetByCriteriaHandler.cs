using Application.Features.Common.Interfaces;
using Application.Features.Common.Specifications;
using Application.Features.Medias.Games.Common;
using Domain.Aggregate;
using Domain.Specification;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Medias.Games.GetByCriteria
{
    internal class GetByCriteriaHandler : IRequestHandler<GetByCriteriaQuery, List<GameResponse>>
    {
        private readonly IAppDbContext appDbContext;

        public GetByCriteriaHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<List<GameResponse>> Handle(GetByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var gamesQuery = appDbContext.Medias.OfType<Game>().AsNoTracking().AsQueryable();
            var genresDictionary = await appDbContext.Genres.AsNoTracking().ToDictionaryAsync(g => g.Id, g => g, cancellationToken);
            List<Guid>? searchGenresId = null;
            if (!string.IsNullOrWhiteSpace(request.genreName))
            {
                searchGenresId = genresDictionary
                    .Where(g => g.Value.Name.Contains(request.genreName))
                    .Select(g => g.Key)
                    .ToList();
            }
            var specification = new GameFilterSpecification(request.title,searchGenresId,request.platform,request.developer,request.releaseDate,request.MinRating,request.sortByField,request.IsDescending);
            var result = SpecificationEvaluator.GetQuery(gamesQuery, specification);
            var response = await result.Select(g=>GameMapper.ToGameResponse(g, genresDictionary.GetValueOrDefault(g.GenreId)!)).ToListAsync(cancellationToken);
            return response;
        }
    }
}
