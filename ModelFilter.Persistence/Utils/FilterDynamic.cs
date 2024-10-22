using ModelFilter.Domain.Interface;
using ModelFilter.Domain.Utils.Filters;
using System.Linq.Expressions;

namespace ModelFilter.Persistence.Utils
{
    public class FilterDynamic : IFilterDynamic
    {
        private readonly IFilterInterpreterFactory _filterInterpreterFactory;

        public FilterDynamic(IFilterInterpreterFactory filterInterpreterFactory)
        {
            _filterInterpreterFactory = filterInterpreterFactory;
        }

        public Expression<Func<TType, bool>> FromFilterList<TType>(FilterBase filter)
        {
            var expression = filter.Filters.Select(x =>
            {
                var interpreter = _filterInterpreterFactory.Create<TType>(x);

                return interpreter;
            });
            var aggregate = expression.Aggregate((curr, next) => curr.And(next));

            return aggregate.Interpret();
        }
    }
}
