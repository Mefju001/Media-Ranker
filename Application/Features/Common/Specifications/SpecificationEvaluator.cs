using Domain.Interfaces;

namespace Application.Features.Common.Specifications
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity>(IQueryable<TEntity> inputQuery, ISpecification<TEntity> spec) where TEntity : MediaInfo
        {
            var query = inputQuery;
            foreach (var criteria in spec.Criteria)
            {
                query = query.Where(criteria);
            }
            if(spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            return query;
        }
    }
}

