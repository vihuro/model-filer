using ModelFilter.Domain.Utils.Filters;
using System.Linq.Expressions;

namespace ModelFilter.Domain.Interface
{
    public interface IFilterDynamic
    {
        Expression<Func<TType, bool>> FromFilterList<TType>(FilterBase filter);
    }
}
