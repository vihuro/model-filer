using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Models;
using ModelFilter.Domain.Utils.Filters.Expressions;

namespace ModelFilter.Domain.Utils.Filters
{
    public class FilterInterpreterFactory : IFilterInterpreterFactory
    {
        public IFilterTypeInterpreter<TType> Create<TType>(Filter filtroItem)
        {
            switch (filtroItem.Operation)
            {
                case FilterTypeConstants.Equals:
                    return new EqualsInterpreter<TType>(filtroItem);
                case FilterTypeConstants.Contains:
                    return new ContainsInterpreter<TType>(filtroItem);
                case FilterTypeConstants.GreaterThan:
                    return new GreaterThanInterpreter<TType>(filtroItem);
                case FilterTypeConstants.GreaterThanOrEqual:
                    return new GreaterThanOrEqualInterpreter<TType>(filtroItem);
                //case FilterTypeConstants.LessThan:
                //    return new LessThanInterpreter<TType>(filtroItem);
                //case FilterTypeConstants.StartsWith:
                //    return new StartsWithInterpreter<TType>(filtroItem);

                default:
                    throw new ArgumentException($"the filter type {filtroItem.Operation} is invalid");
            }
        }
    }
}
