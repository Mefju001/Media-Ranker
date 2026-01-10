using MediatR;
using System.Linq.Expressions;
using WebApplication1.Application.Common.DTO.Response;
using WebApplication1.Application.Features.TvSeries.GetTvSeriesByCriteria;
using WebApplication1.Domain.Entities;
using WebApplication1.Infrastructure.Specification;

namespace WebApplication1.QueryHandler
{
    public class TvSeriesQueryhandler(QueryServices<TvSeries> queryServices) : IRequestHandler<TvSeriesQuery, List<TvSeriesResponse>>
    {
        private readonly QueryServices<TvSeries> queryServices = queryServices;
        public Task<List<TvSeriesResponse>> Handle(TvSeriesQuery request, CancellationToken cancellationToken)
        {
            var query = queryServices.StartQuery();
            var predicate = buildPredicate(request);
            if (!string.IsNullOrEmpty(request.sortByField) || request.isDescending)
            {
                query = queryServices.Sort(query, request.sortByField, request.isDescending);
            }
            throw new NotImplementedException();
        }
        private Expression<Func<TvSeries, bool>> buildPredicate(TvSeriesQuery query)
        {
            var predicate = PredicateBuilder.True<TvSeries>();
            if (!string.IsNullOrEmpty(query.title))
            {
                predicate = predicate.And(tv => tv.title.Contains(query.title));
            }
            if (!string.IsNullOrEmpty(query.genreName))
            {
                predicate = predicate.And(tv => tv.genre.name.Contains(query.genreName));
            }
            if (!string.IsNullOrEmpty(query.network))
            {
                predicate = predicate.And(tv => tv.Network.Contains(query.network));
            }
            if (query.seasons.HasValue)
            {
                predicate = predicate.And(tv => tv.Seasons >= query.seasons);
            }
            if (query.episodes.HasValue)
            {
                predicate = predicate.And(tv => tv.Episodes >= query.episodes);
            }
            return predicate;
        }
    }
}
