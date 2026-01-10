using System.Linq.Expressions;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Mapper;

using MediatR;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain.Entities;
using Application.Common.Interfaces;
using Application.Features.TvSeriesManagement.GetTvSeriesByCriteria;


namespace WebApplication1.Application.Features.TvSeries.GetMoviesByCriteria
{
    public class GetTvSeriesByCriteriaHandler : IRequestHandler<GetTvSeriesByCriteriaQuery, List<TvSeriesResponse>>
    {
        private readonly IAppDbContext context;
        private readonly ISorterContext<Domain.Entities.TvSeries> sorterContext;
        private readonly ITvSeriesFilter tvSeriesFilter;
        private readonly IBuildPredicate buildPredicate;

        public GetTvSeriesByCriteriaHandler(ISorterContext<Domain.Entities.TvSeries> sorterContext, IAppDbContext appDbContext, IBuildPredicate buildPredicate, ITvSeriesFilter tvSeriesFilter)
        {
            this.sorterContext = sorterContext;
            this.context = appDbContext;
            this.buildPredicate = buildPredicate;
            this.tvSeriesFilter = tvSeriesFilter;
        }


        public async Task<List<TvSeriesResponse>> Handle(GetTvSeriesByCriteriaQuery request, CancellationToken cancellationToken)
        {
            var query = context.TvSeries
                .Include(m => m.genre)
                .Include(m => m.Stats)
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
