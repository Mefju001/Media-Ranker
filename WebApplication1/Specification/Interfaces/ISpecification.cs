using System.Linq.Expressions;

namespace WebApplication1.Specification.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
    }
}
