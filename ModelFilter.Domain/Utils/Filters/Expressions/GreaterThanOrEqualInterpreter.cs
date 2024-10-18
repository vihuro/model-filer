using System.Linq.Expressions;

namespace ModelFilter.Domain.Utils.Filters.Expressions
{
    public class GreaterThanOrEqualInterpreter<T> : FilterTypeInterpreter<T>
    {
        public GreaterThanOrEqualInterpreter(Filter filter) : base(filter)
        {
        }

        internal override Expression CreateExpression(MemberExpression property, ConstantExpression constant)
        {
            var expression = Expression.GreaterThanOrEqual(property, constant);

            return expression;
        }
    }
}
