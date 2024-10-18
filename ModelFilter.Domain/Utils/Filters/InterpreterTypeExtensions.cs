using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Utils.Filters.Expressions;

namespace ModelFilter.Domain.Utils.Filters
{
    public static class InterpreterTypeExtensions
    {
        public static IFilterTypeInterpreter<TType> And<TType>(this IFilterTypeInterpreter<TType> left, IFilterTypeInterpreter<TType> right)
                      => new AndInterpreter<TType>(left, right);

        public static IFilterTypeInterpreter<TType> Or<TType>(this IFilterTypeInterpreter<TType> left, IFilterTypeInterpreter<TType> right)
                      => new OrInterpreter<TType>(left, right);
    }
}
