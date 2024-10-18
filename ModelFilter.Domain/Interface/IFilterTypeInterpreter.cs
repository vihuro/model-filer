using System.Linq.Expressions;

namespace ModelFilter.Domain.Interface
{
    public interface IFilterTypeInterpreter<TType>
    {
        Expression<Func<TType, bool>> Interpret();
    }
}
