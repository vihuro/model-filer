using System.Linq.Expressions;

namespace ModelFilter.Domain.Utils.Filters.Expressions
{
    public class EqualsInterpreter<T> : FilterTypeInterpreter<T>
    {
        public EqualsInterpreter(Filter filter) : base(filter)
        {
        }

        internal override Expression CreateExpression(MemberExpression property, ConstantExpression constant)
        {
            var expression = Expression.Equal(property, constant);

            return expression;
        }

    }
}
