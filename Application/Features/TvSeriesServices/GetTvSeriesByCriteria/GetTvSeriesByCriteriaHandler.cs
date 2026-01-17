using Application.Common.DTO.Response;
using Application.Common.Interfaces;
using Application.Mapper;
using Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Features.TvSeriesServices.GetTvSeriesByCriteria
{
    public class GetTvSeriesByCriteriaHandler : IRequestHandler<GetTvSeriesByCriteriaQuery, List<TvSeriesResponse>>
    {
        private readonly IAppDbContext context;
        private readonly ISorterContext<TvSeriesDomain> sorterContext;
        private readonly ITvSeriesFilter tvSeriesFilter;
        private readonly ITvSeriesBuildPredicate buildPredicate;

        public GetTvSeriesByCriteriaHandler(ISorterContext<TvSeriesDomain> sorterContext, IAppDbContext appDbContext, ITvSeriesBuildPredicate buildPredicate, ITvSeriesFilter tvSeriesFilter)
        {
            this.sorterContext = sorterContext;
            this.context = appDbContext;
            this.buildPredicate = buildPredicate;
            this.tvSeriesFilter = tvSeriesFilter;
        }


        public async Task<List<TvSeriesResponse>> Handle(GetTvSeriesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = context.TvSeries
                //.Include(m => m.genre)
                //.Include(m => m.Stats)
                .AsNoTracking()
                .AsQueryable();
            var predicate = buildPredicate.build(request);
            query = tvSeriesFilter.Filter(query, predicate);
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
            var Response = movies.Select(m => TvSeriesMapper.ToTvSeriesResponse(m)).ToList();
            return Response;
        }


    }
}
