using System.Linq.Expressions;
using WebApplication1.Specification.Interfaces;

namespace WebApplication1.Specification
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        private readonly Expression<Func<T, bool>> predicate;
        protected BaseSpecification(Expression<Func<T, bool>> predicate)
        {
            this.predicate = predicate;
        }
        public Expression<Func<T, bool>> ToExpression()
        {
            return predicate;
        }
    }
}
