using System.Linq.Expressions;

namespace WebApplication1.Application.Common.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> ToExpression();
    }
}
