using System.Linq.Expressions;

namespace Application.Common.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
    }
}
