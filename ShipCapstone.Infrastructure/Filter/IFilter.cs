using System.Linq.Expressions;

namespace ShipCapstone.Infrastructure.Filter;

public interface IFilter<T>
{
    Expression<Func<T, bool>> ToExpression();
}