using Application.Features.Common.Interfaces;
using Application.Features.Common.Specifications;
using Application.Features.Medias.TvSeries.Common;
using Domain.Specification;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.Medias.TvSeries.GetByCriteria
{
    internal class GetByCriteriaHandler : IRequestHandler<GetByCriteriaQuery, List<TvSeriesResponse>>
    {
        private readonly IAppDbContext appDbContext;

        public GetByCriteriaHandler(IAppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }


        public async Task<List<TvSeriesResponse>> Handle(GetByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = appDbContext.Medias.OfType<Domain.Aggregate.TvSeries>().AsNoTracking().AsQueryable();
            var genresDictionary = await appDbContext.Genres.AsNoTracking()
                .ToDictionaryAsync(g=>g.Id, g=>g, cancellationToken);
            List<Guid>? searchGenresId = null;
            if (!string.IsNullOrWhiteSpace(request.genreName))
            {
                searchGenresId = genresDictionary
                    .Where(g => g.Value.Name.Contains(request.genreName))
                    .Select(g => g.Key)
                    .ToList();
            }
            var criteria = new TvSeriesFilterSpecification
            (
                request.TitleSearch,
                request.MinRating,
                request.ReleaseYear,
                searchGenresId,
                request.network,
                request.status,
                request.SortByField,
                request.IsDescending
            );
            var result = SpecificationEvaluator.GetQuery(query, criteria);
            var response = await result.Select(x => TvSeriesMapper.ToTvSeriesResponse(x, genresDictionary.GetValueOrDefault(x.GenreId)!)).ToListAsync(cancellationToken);
            return  response;
        }
    }
}
