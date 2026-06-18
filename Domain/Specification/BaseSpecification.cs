using Domain.Interfaces;
using System.Linq.Expressions;

namespace Domain.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T: MediaInfo
    {
        public List<Expression<Func<T, bool>>> Criteria { get; } = new();
        public Expression<Func<T, object>>? OrderBy {  get; private set; }
        public Expression<Func<T, object>>? OrderByDescending {  get; private set; }

        protected void AddCriteria(Expression<Func<T, bool>> criterion) => Criteria.Add(criterion);

        protected void ApplyOrderBy(Expression<Func<T, object>> orderBy) => OrderBy = orderBy;
        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDesc) => OrderByDescending = orderByDesc;
    }
}
