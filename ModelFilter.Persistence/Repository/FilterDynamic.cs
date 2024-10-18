using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Utils.Filters;
using System.Linq.Expressions;

namespace ModelFilter.Persistence.Repository
{
    public class FilterDynamic : IFilterDynamic
    {
        private readonly IFilterInterpreterFactory _filterInterpreterFactory;

        public FilterDynamic(IFilterInterpreterFactory filterInterpreterFactory)
        {
            _filterInterpreterFactory = filterInterpreterFactory;
        }

        public Expression<Func<TType, bool>> FromFilterList<TType>(FilterBaseDto filter)
        {
            var expression = filter.Filters.Select(x =>
            {
                var interpreter = _filterInterpreterFactory.Create<TType>(x);
                var resolveInterpreter = ResolveNextInterpreter(interpreter, x);

                return resolveInterpreter;
            });
            var aggregate = expression.Aggregate((curr, next) => curr.And(next));

            return aggregate.Interpret();
        }
        private IFilterTypeInterpreter<TType> ResolveNextInterpreter<TType>(IFilterTypeInterpreter<TType> interpreter, Filter filtroItem)
        {

            //if (filtroItem.Or != null)
            //    return interpreter.Or(_factory.Create<TType>(filtroItem.Or));

            //if (filtroItem.And != null)
            //    return interpreter.And(_factory.Create<TType>(filtroItem.And));

            return interpreter;
        }
    }
}
